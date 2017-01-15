/*
 * Author(s): Isaiah Mann
 * Description: Used to control the audio in the game
 * Is a Singleton (only one instance can exist at once)
 * Attached to a GameObject that stores all AudioSources and AudioListeners for the game
 * Dependencies: AudioFile, AudioLoader, AudioList, AudioUtil, RandomizedQueue<AudioFile>
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioController : Controller, IAudioController
{
	#region Static Accessors

	// Singleton implementation
	public static AudioController Instance
	{
		get 
		{
			return _instance;
		}
	}

	#endregion

	private static AudioController _instance;

	[SerializeField]
	bool isAudioListener = false;
	// Set to false to halt active coroutines
	[SerializeField]
	bool playMusicOnInit;
	[SerializeField]
	bool useCustomPaths = false;

	[SerializeField]
	string mainMusicEventName;
	[SerializeField]
	string customJSONPath;
	[SerializeField]
	string customAudioPath;

	bool coroutinesActive = true;

	AudioList fileList;
	AudioLoader loader;

	// Stores all the audio sources and audio data inside dictionaries
	Dictionary<int, AudioSource> channels = new Dictionary<int, AudioSource>();
	Dictionary<string, AudioData> data = new Dictionary<string, AudioData>();

	// Stores all the audio events inside dictionaries
	Dictionary<string, List<AudioData>> playEvents = new Dictionary<string, List<AudioData>>();
	Dictionary<string, List<AudioData>> stopEvents = new Dictionary<string, List<AudioData>>();

	// Audio Control Patterns
	RandomizedQueue<AudioFile> swells;
	RandomizedQueue<AudioFile> sweeteners;
	IEnumerator swellCoroutine;
	IEnumerator sweetenerCoroutine;

	// Backup channels that are dynamically created to allow SFX to finish without being cut off (empty when no SFX are playing)
	List<AudioSource> tempSFXChannels = new List<AudioSource>();

	[Header("Sweeteners")]
	[SerializeField]
	float shortestSweetenerPlayFrequenecy = 10;
	[SerializeField]
	float longestSweetenerPlayFrequenecy = 25;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		init();
	}

	protected override void cleanupReferences()
	{
		base.cleanupReferences();
		unsubscribeEvents();
	}

	protected override void handleNamedEvent(string eventName)
	{
		if(playEvents.ContainsKey(eventName)) 
		{
			playAudioList(playEvents[eventName]);
		}
		if(stopEvents.ContainsKey(eventName)) 
		{
			stopAudioList(stopEvents[eventName]);
		}
	}

	// Uses C#'s delegate system
	protected override void subscribeEvents() {
		base.subscribeEvents();
		EventController.Subscribe(handleAudioEvent);
	}

	protected override void unsubscribeEvents() {
		base.unsubscribeEvents();
		EventController.Unsubscribe(handleAudioEvent);
	}

	#endregion

	public void Play(AudioFile file) 
	{
		AudioSource source = getChannel(file.Channel);
		checkMute(file, source);
		bool shouldResumeClip = false;
		float clipTime = 0;
		if(file.Type == AudioType.FX) 
		{
			if(source.clip != null && source.isPlaying) 
			{ 
				if(!AudioUtil.IsMuted(AudioType.FX)) 
				{
					StartCoroutine(completeOnTempChannel(source.clip, source.time, source.volume));
				}
			}
		}
		else if(file.Type == AudioType.Music) 
		{
			if(source.clip == file.Clip) 
			{
				shouldResumeClip = true;
				clipTime = source.time;
			}
		}
		source.clip = file.Clip;
		source.loop = file.Loop;
		source.volume = file.Volumef;
		if(shouldResumeClip) 
		{
			source.time = clipTime;
		}
		source.Play();
	}

	public void Stop(AudioFile file) 
	{
		if(channelExists(file.Channel)) 
		{
			AudioSource source = getChannel(file.Channel);
			if(source.clip == file.Clip) 
			{
				source.Stop();
			}
		}
	}

	public void ToggleFXMute() 
	{
		SettingsUtil.ToggleSFXMuted(!SettingsUtil.SFXMuted);
		if(SettingsUtil.SFXMuted) 
		{
			StopAllCoroutines();
			teardownTempSFXChannels();
		}
	}

	public void ToggleMusicMute()
	{
		SettingsUtil.ToggleMusicMuted(!SettingsUtil.MusicMuted);
	}

	void checkMute(AudioFile file, AudioSource source) 
	{
		source.mute = AudioUtil.IsMuted(file.Type);
	}

	// Checks if the AudioSource corresponding to the channel integer has been initialized
	bool channelExists(int channelNumber) 
	{
		return channels.ContainsKey(channelNumber);
	}

	AudioSource getChannel(int channelNumber) 
	{
		if(channels.ContainsKey(channelNumber)) 
		{
			return channels[channelNumber];
		}
		else
		{
			// Adds a new audiosource if channel is not present in dictionary
			AudioSource newSource = gameObject.AddComponent<AudioSource>();
			channels.Add(channelNumber, newSource);
			return newSource;
		}
	}

	// Must be colled to setup the class's functionality
	void init() {
		// Singleton method returns a bool depending on whether this object is the instance of the class
		if(SingletonUtil.TryInit(ref _instance, this, gameObject, true)) 
		{
			if(useCustomPaths)
			{
				loader = new AudioLoader(customJSONPath, customAudioPath);
			}
			else
			{
				loader = AudioLoader.Default;
			}
			fileList = loader.Load();
			if(!fileList.AreEventsSubscribed) 
			{
				fileList.SubscribeEvents();
			}
			fileList.PopulateGroups();
			initFileDictionary(fileList);
			addAudioEvents();
			subscribeEvents();
			if(isAudioListener) 
			{
				addAudioListener();
			}
			preloadFiles(fileList.Files);
			if(playMusicOnInit) 
			{
				playMainMusic();
			}
		}
	}

	void initFileDictionary(AudioList audioFiles)
	{
		for(int i = 0; i < audioFiles.Length; i++) 
		{
			data.Add(audioFiles[i].Name, audioFiles[i]);
		}
	}

	void addAudioEvents() 
	{
		for(int i = 0; i < fileList.Length; i++) 
		{
			addPlayEvents(fileList[i]);
			addStopEvents(fileList[i]);
		}
		addGroupEvents(fileList);
	}

	void addGroupEvents(AudioList list) 
	{
		AudioGroup[] groups = list.Groups;
		for(int i = 0; i < groups.Length; i++) 
		{
			addPlayEvents(groups[i]);
			addStopEvents(groups[i]);
		}
			
	}

	void addPlayEvents(AudioData file) 
	{
		for(int j = 0; j < file.Events.Length; j++) 
		{
			if(playEvents.ContainsKey(file.Events[j])) 
			{
				playEvents[file.Events[j]].Add(file);
			} 
			else 
			{
				List<AudioData> files = new List<AudioData>();
				files.Add(file);
				playEvents.Add(file.Events[j], files);
			}
		}
	}

	void addStopEvents(AudioData file) 
	{
		for(int j = 0; j < file.StopEvents.Length; j++) 
		{
			if(stopEvents.ContainsKey(file.StopEvents[j])) 
			{
				stopEvents[file.StopEvents[j]].Add(file);
			} 
			else 
			{
				List<AudioData> files = new List<AudioData>();
				files.Add(file);
				stopEvents.Add(file.StopEvents[j], files);
			}
		}
	}
		
	void playMainMusic() {
		EventController.Event(mainMusicEventName);
	}

	void handleAudioEvent(AudioActionType actionType, AudioType audioType) 
	{
		if(AudioUtil.IsMuteAction(actionType)) 
		{
			handleMuteAction(actionType, audioType);
		}
	}

	void handleMuteAction(AudioActionType actionType, AudioType audioType) 
	{
		foreach(AudioSource source in channels.Values) 
		{
			if(fileList.GetAudioType(source.clip) == audioType) 
			{
				source.mute = AudioUtil.MutedBoolFromAudioAction(actionType);
			}
		}
	}

	void playAudioList(List<AudioData> data) 
	{
		for(int i = 0; i < data.Count; i++) 
		{
			Play(data[i].GetNextFile());
		}
	}

	void stopAudioList(List<AudioData> data) 
	{
		for(int i = 0; i < data.Count; i++) 
		{
			if(data[i].HasCurrentFile()) 
			{
				Stop(data[i].GetCurrentFile());
			}
		}
	}

	void addAudioListener() 
	{
		gameObject.AddComponent<AudioListener>();
	}

	// Used to loop through lists of tracks in pseudo-random order
	void startTrackCycling()
	{
		sweetenerCoroutine = cycleTracksFrequenecyRange
		(
			sweeteners,
			shortestSweetenerPlayFrequenecy,
			longestSweetenerPlayFrequenecy
		);

		swellCoroutine = cycleTracksContinuous(swells);
		startCoroutines(sweetenerCoroutine, swellCoroutine);
	}

	void initCyclingAudio()
	{
		sweeteners = new RandomizedQueue<AudioFile>();
		swells = new RandomizedQueue<AudioFile>();
		startTrackCycling();
	}

	// Plays audio files back to back
	IEnumerator cycleTracksContinuous(RandomizedQueue<AudioFile> files) 
	{
		while(coroutinesActive) 
		{
			AudioFile nextTrack = files.Cycle();
			Play(nextTrack);
			yield return new WaitForSeconds(nextTrack.Clip.length);
		}
	}

	// Plays audio files on a set frequenecy
	IEnumerator cycleTracksFrequenecy(RandomizedQueue<AudioFile> files, float frequenecy) 
	{
		while(coroutinesActive) 
		{
			Play(files.Cycle());
			yield return new WaitForSeconds(frequenecy);
		}
	}

	// Coroutine that varies the frequency with which it plays audio files
	IEnumerator cycleTracksFrequenecyRange(RandomizedQueue<AudioFile> files, float minFrequency, float maxFrequency) 
	{
		while(coroutinesActive) 
		{
			Play(files.Cycle());
			yield return new WaitForSeconds(UnityEngine.Random.Range(minFrequency, maxFrequency));
		}
	}
		
	IEnumerator completeOnTempChannel(AudioClip clip, float timeStamp, float volume) 
	{
		AudioSource tempChannel = null;
		AudioType clipType = fileList.GetAudioType(clip);
		float remainingTime = clip.length - timeStamp;
		try 
		{
			if(remainingTime > 0) 
			{
				tempChannel = gameObject.AddComponent<AudioSource>();
				if(clipType == AudioType.FX) 
				{
					tempSFXChannels.Add(tempChannel);
				}
				tempChannel.clip = clip;
				tempChannel.time = Mathf.Clamp(timeStamp, 0, clip.length);
				tempChannel.volume = volume;
				tempChannel.Play();
			}
		} 
		catch 
		{
			Debug.LogFormat("Seek time is this: {0} and the clip length is this: {1}", timeStamp, clip.length);
		}
		yield return new WaitForSeconds(remainingTime);
		if(tempChannel != null) 
		{
			if(clipType == AudioType.FX) 
			{
				tempSFXChannels.Remove(tempChannel);
			}
			Destroy(tempChannel);
		}
	}

	// Preloads certain audio files 
	void preloadFiles(params AudioFile[] audioFiles) 
	{
		for(int i = 0; i < audioFiles.Length; i++) 
		{
			StartCoroutine(preloadAudioClip(audioFiles[i]));
		}
	}

	// Instantly kills temp channels (standard use is for mute toggled on)
	void teardownTempSFXChannels() 
	{
		for(int i = 0; i < tempSFXChannels.Count; i++) 
		{
			tempSFXChannels[i].Stop();
			Destroy(tempSFXChannels[i]);
		}
		tempSFXChannels.Clear();
	}

	// Asynchronous loading to improve game load times
	IEnumerator preloadAudioClip(AudioFile audioFile) 
	{
		ResourceRequest request = loader.GetClipAsync(audioFile);
		while(!request.isDone) 
		{
			if(audioFile.ClipIsSet()) 
			{
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
		if(!audioFile.ClipIsSet()) 
		{
			try 
			{
				audioFile.SetClip((AudioClip) request.asset);
			} 
			catch(Exception e) 
			{
				Debug.LogError(e + ": " + audioFile.Name + " is not a valid AudioClip");
			}
		}
	}

}
