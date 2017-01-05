/*
 * Author(s): Isaiah Mann 
 * Description: Wrapper class to store a list of AudioFiles
 * Has array-like properties
 * Dependencies: AudioFile
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioList 
{

	#region Instance Accessors

	public AudioFile this[int index] 
	{
		get
		{
			return Files[index];
		}
	}
	public int Length 
	{
		get 
		{
			if(Files == null) 
			{ 
				return 0;
			} 
			else 
			{
				return Files.Length;
			}
		}
	}

	public bool AreEventsSubscribed 
	{
		get;
		private set;
	}

	#endregion

	public AudioFile[] Files;
	public AudioGroup[] Groups;

	Dictionary<AudioClip, AudioFile> clipToFileDictionary = new Dictionary<AudioClip, AudioFile>();

	public AudioList(AudioFile[] files) 
	{
		Files = files;
		SubscribeEvents();
	}

	public AudioList()
	{
		// NOTHING
	}
		
	// Destructor for Garbage Collection:
	~AudioList()
	{
		unsubscribeEvents();
	}
		

	public void PopulateGroups() 
	{
		foreach(AudioFile file in Files) 
		{
			foreach(AudioGroup group in Groups) 
			{
				if(ArrayUtil.Contains(file.Groups, group.Name)) 
				{
					group.AddFile(file);
				}
			}
		}
	}

	public AudioType GetAudioType(AudioClip clip) 
	{
		return AudioUtil.AudioTypeFromString(clipToFileDictionary[clip].Type);
	}

	void processAudioFileAccess(AudioFile file) 
	{
		addToClipDictionary(file);
	}

	void addToClipDictionary(AudioFile file) 
	{
		if(!clipToFileDictionary.ContainsKey(file.Clip)) 
		{
			clipToFileDictionary.Add(file.Clip, file);
		}
	}

	public void SubscribeEvents() 
	{
		for (int i = 0; i < Files.Length; i++) 
		{
			Files[i].OnClipRequest += processAudioFileAccess;
		}
		AreEventsSubscribed = true;
	}

	void unsubscribeEvents() 
	{
		for(int i = 0; i < Files.Length; i++) 
		{
			Files[i].OnClipRequest -= processAudioFileAccess;
		}
	}

	void handleClipRequest(AudioFile file) 
	{
		processAudioFileAccess(file);
	}		

}
