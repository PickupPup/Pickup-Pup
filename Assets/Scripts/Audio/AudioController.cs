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

public class AudioController : Controller, IAudioController {
	public bool isAudioListener = true;

	// For controlling music
	[SerializeField]
	bool playMusicOnInit;
	[SerializeField]
	string mainMusicEventName;

	// Singleton implementation
	public static AudioController Instance;

	const string path = "Audio/AudioList";
	AudioList fileList;
	AudioLoader loader;

	// Stores all the audio sources and audio data inside dictionaries
	Dictionary<int, AudioSource> channels = new Dictionary<int, AudioSource>();
	Dictionary<string, AudioData> data = new Dictionary<string, AudioData>();

	// Stores all the audio events inside dictionaries
	Dictionary<string, List<AudioData>> playEvents = new Dictionary<string, List<AudioData>>();
	Dictionary<string, List<AudioData>> stopEvents = new Dictionary<string, List<AudioData>>();

	// Audio Control Patterns
	RandomizedQueue<AudioFile> _swells;
	RandomizedQueue<AudioFile> _sweeteners;
	IEnumerator _swellCoroutine;
	IEnumerator _sweetenerCoroutine;

	// Backup channels that are dynamically created to allow SFX to finish without being cut off (empty when no SFX are playing)
	List<AudioSource> tempSFXChannels = new List<AudioSource>();

	// Set to false to halt active coroutines
	bool _coroutinesActive = true;
	[Header("Sweeteners")]
	public float ShortestSweetenerPlayFrequenecy = 10;
	public float LongestSweetenerPlayFrequenecy = 25;

	protected override void SetReferences () {
		Init();
	}

	protected override void FetchReferences () {

	}

	protected override void CleanupReferences () {
		UnsubscribeEvents();
	}

	protected override void HandleNamedEvent (string eventName) {
		if (playEvents.ContainsKey(eventName)) {
			PlayAudioList(
				playEvents[eventName]
			);
		}
		if (stopEvents.ContainsKey(eventName)) {
			StopAudioList(
				stopEvents[eventName]
			);
		}
	}

	public void Play (AudioFile file) {
		AudioSource source = GetChannel(file.Channel);
		CheckMute(file, source);
		bool shouldResumeClip = false;
		float clipTime = 0;
		if (file.TypeAsEnum == AudioType.FX) {
			if (source.clip != null && source.isPlaying) { 
				if (!AudioUtil.IsMuted(AudioType.FX)) {
					StartCoroutine(CompleteOnTempChannel(source.clip, source.time, source.volume));
				}
			}
		} else if (file.TypeAsEnum == AudioType.Music) {
			if (source.clip == file.Clip) {
				shouldResumeClip = true;
				clipTime = source.time;
			}
		}
		source.clip = file.Clip;
		source.loop = file.Loop;
		source.volume = file.Volumef;
		if (shouldResumeClip) {
			source.time = clipTime;
		}
		source.Play();
	}

	public void Stop (AudioFile file) {
		if (ChannelExists(file.Channel)) {
			AudioSource source = GetChannel(file.Channel);
			if (source.clip == file.Clip) {
				source.Stop();
			}
		}
	}

	public void ToggleFXMute () {
		SettingsUtil.ToggleSFXMuted (
			!SettingsUtil.SFXMuted
		);
		if (SettingsUtil.SFXMuted) {
			StopAllCoroutines();
			TeardownTempSFXChannels();
		}
	}

	public void ToggleMusicMute () {
		SettingsUtil.ToggleMusicMuted (
			!SettingsUtil.MusicMuted
		);
	}

	void CheckMute (AudioFile file, AudioSource source) {
		source.mute = AudioUtil.IsMuted(file.TypeAsEnum);
	}

	// Checks if the AudioSource corresponding to the channel integer has been initialized
	bool ChannelExists (int channelNumber) {
		return channels.ContainsKey(channelNumber);
	}

	AudioSource GetChannel (int channelNumber) {
		if (channels.ContainsKey(channelNumber)) {
			return channels[channelNumber];
		} else {
			// Adds a new audiosource if channel is not present in dictionary
			AudioSource newSource = gameObject.AddComponent<AudioSource>();
			channels.Add(channelNumber, newSource);
			return newSource;
		}
	}


	// Must be colled to setup the class's functionality
	void Init () {
		// Singleton method returns a bool depending on whether this object is the instance of the class
		if (SingletonUtil.TryInit(ref Instance, this, gameObject, true)) {
			loader = new AudioLoader(path);
			fileList = loader.Load();
			if (!fileList.AreEventsSubscribed) {
				fileList.SubscribeEvents();
			}
			fileList.PopulateGroups();
			InitFileDictionary(fileList);
			AddAudioEvents();
			SubscribeEvents();
			if (isAudioListener) {
				AddAudioListener();
			}
			PreloadFiles(fileList.Files);
			// TODO: Enable after tracks have been delivered
			// initCyclingAudio();
			if (playMusicOnInit) {
				playMainMusic();
			}
		}
	}

	void InitFileDictionary (AudioList audioFiles) {
		for (int i = 0; i < audioFiles.Length; i++) {
			data.Add (
				audioFiles[i].Name,
				audioFiles[i]
			);
		}
	}

	void AddAudioEvents () {
		for (int i = 0; i < fileList.Length; i++) {
			AddPlayEvents(fileList[i]);
			AddStopEvents(fileList[i]);
		}
		AddGroupEvents(fileList);
	}

	void AddGroupEvents (AudioList list) {
		AudioGroup[] groups = list.Groups;
		for (int i = 0; i < groups.Length; i++) {
			AddPlayEvents(groups[i]);
			AddStopEvents(groups[i]);
		}
			
	}

	void AddPlayEvents (AudioData file) {
		for (int j = 0; j < file.Events.Length; j++) {
			if (playEvents.ContainsKey(file.Events[j])) {
				playEvents[file.Events[j]].Add(file);
			} else {
				List<AudioData> files = new List<AudioData>();
				files.Add(file);
				playEvents.Add (
					file.Events[j],
					files
				);
			}
		}
	}

	void AddStopEvents (AudioData file) {
		for (int j = 0; j < file.StopEvents.Length; j++) {
			if (stopEvents.ContainsKey(file.StopEvents[j])) {
				stopEvents[file.StopEvents[j]].Add(file);
			} else {
				List<AudioData> files = new List<AudioData>();
				files.Add(file);
				stopEvents.Add (
					file.StopEvents[j],
					files
				);
			}
		}
	}

	// Uses C#'s delegate system
	protected override void SubscribeEvents () {
		base.SubscribeEvents();
		EventController.OnAudioEvent += HandleAudioEvent;
	}

	protected void UnsubscribeEvents () {
		base.UnusbscribeEvents();
		EventController.OnAudioEvent -= HandleAudioEvent;
	}

	void playMainMusic () {
		EventController.Event(mainMusicEventName);
	}

	void HandleAudioEvent (AudioActionType actionType, AudioType audioType) {
		if (AudioUtil.IsMuteAction(actionType)) {
			HandleMuteAction (actionType, audioType);
		}
	}

	void HandleMuteAction (AudioActionType actionType, AudioType audioType) {
		foreach (AudioSource source in channels.Values) {
			if (fileList.GetAudioType(source.clip) == audioType) {
				source.mute = AudioUtil.MutedBoolFromAudioAction(actionType);
			}
		}
	}

	void PlayAudioList (List<AudioData> data) {
		for (int i = 0; i < data.Count; i++) {
			Play(data[i].GetNextFile());
		}
	}

	void StopAudioList (List<AudioData> data) {
		for (int i = 0; i < data.Count; i++) {
			if (data[i].HasCurrentFile()) {
				Stop(data[i].GetCurrentFile());
			}
		}
	}

	void AddAudioListener () {
		gameObject.AddComponent<AudioListener>();
	}

	// Used to loop through lists of tracks in pseudo-random order
	void startTrackCycling () {
		_sweetenerCoroutine = cycleTracksFrequenecyRange(
			_sweeteners,
			ShortestSweetenerPlayFrequenecy,
			LongestSweetenerPlayFrequenecy
		);

		_swellCoroutine = cycleTracksContinuous (
			_swells
		);

		startCoroutines(
			_sweetenerCoroutine,
			_swellCoroutine
		);
	}

	void initCyclingAudio () {
		//TODO: Init Queue's with sound files once they're delivered
		_sweeteners = new RandomizedQueue<AudioFile>();
		_swells = new RandomizedQueue<AudioFile>();
		startTrackCycling();
	}

	// Plays audio files back to back
	IEnumerator cycleTracksContinuous (RandomizedQueue<AudioFile> files) {
		while (_coroutinesActive) {
			AudioFile nextTrack = files.Cycle();
			Play(nextTrack);
			yield return new WaitForSeconds(nextTrack.Clip.length);
		}
	}

	// Plays audio files on a set frequenecy
	IEnumerator cycleTracksFrequenecy (RandomizedQueue<AudioFile> files, float frequenecy) {
		while (_coroutinesActive) {
			Play(files.Cycle());
			yield return new WaitForSeconds(frequenecy);
		}
	}

	// Coroutine that varies the frequency with which it plays audio files
	IEnumerator cycleTracksFrequenecyRange (RandomizedQueue<AudioFile> files, float minFrequency, float maxFrequency) {
		while (_coroutinesActive) {
			Play(files.Cycle());

			yield return new WaitForSeconds(
				UnityEngine.Random.Range(
					minFrequency,
					maxFrequency
				)
			);
		}
	}


	// Starts an arbitrary amount of coroutines
	void startCoroutines (params IEnumerator[] coroutines) {
		for (int i = 0; i < coroutines.Length; i++) {
			StartCoroutine(coroutines[i]);
		}
	}

	IEnumerator CompleteOnTempChannel (AudioClip clip, float timeStamp, float volume) {
		AudioSource tempChannel = null;
		AudioType clipType = fileList.GetAudioType(clip);
		float remainingTime = clip.length - timeStamp;
		try {
			if (remainingTime > 0) {
				tempChannel = gameObject.AddComponent<AudioSource>();
				if (clipType == AudioType.FX) {
					tempSFXChannels.Add(tempChannel);
				}
				tempChannel.clip = clip;
				tempChannel.time = Mathf.Clamp(timeStamp, 0, clip.length);
				tempChannel.volume = volume;
				tempChannel.Play();
			}
		} catch {
			Debug.LogFormat("Seek time is this: {0} and the clip length is this: {1}", timeStamp, clip.length);
		}
		yield return new WaitForSeconds(remainingTime);
		if (tempChannel != null) {
			if (clipType == AudioType.FX) {
				tempSFXChannels.Remove(tempChannel);
			}
			Destroy(tempChannel);
		}
	}

	// Preloads certain audio files 
	void PreloadFiles (params AudioFile[] audioFiles) {
		for (int i = 0; i < audioFiles.Length; i++) {
			StartCoroutine(PreloadAudioClip(audioFiles[i]));
		}
	}

	// Instantly kills temp channels (standard use is for mute toggled on)
	void TeardownTempSFXChannels () {
		for (int i = 0; i < tempSFXChannels.Count; i++) {
			tempSFXChannels[i].Stop();
			Destroy(tempSFXChannels[i]);
		}
		tempSFXChannels.Clear();
	}

	// Asynchronous loading to improve game load times
	IEnumerator PreloadAudioClip (AudioFile audioFile) {
		ResourceRequest request = AudioLoader.GetClipAsync(audioFile.Name);
		while (!request.isDone) {
			if (audioFile.ClipIsSet()) {
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
		if (!audioFile.ClipIsSet()) {
			try {
				audioFile.SetClip((AudioClip) request.asset);
			} catch (Exception e) {
				Debug.LogError(e + ": " + request.asset + " is not a valid AudioClip");
			}
		}
	}

	#region JSON Deserialization

	public void DeserializeFromJSON (string jsonText) {
		throw new System.NotImplementedException();
	}

	public void DeserializeFromJSONAtPath (string jsonPath) {
		throw new System.NotImplementedException();
	}

	#endregion
}
