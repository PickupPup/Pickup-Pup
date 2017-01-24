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
			return files[index];
		}
	}

	public AudioFile[] Files 
	{
		get 
		{
			return this.files;
		}
	}
		
	public AudioGroup[] Groups
	{
		get 
		{
			return this.groups;
		}
	}

	public int Length 
	{
		get 
		{
			if(files == null) 
			{ 
				return 0;
			} 
			else 
			{
				return files.Length;
			}
		}
	}

	public bool AreEventsSubscribed 
	{
		get;
		private set;
	}

	#endregion

	AudioData[] allData
	{
		get 
		{
			if(this._allData == null)
			{
				this._allData = ArrayUtil.Concat<AudioData>(files, groups);
			}
			return this._allData;
		}
	}

	AudioData[] _allData;

	[SerializeField]
	AudioFile[] files;
	[SerializeField]
	AudioGroup[] groups;

	Dictionary<AudioClip, AudioFile> clipToFileDictionary = new Dictionary<AudioClip, AudioFile>();

	public AudioList(AudioFile[] files) 
	{
		this.files = files;
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

	public void SetLoader(AudioLoader loader)
	{
		foreach(AudioData audio in allData) 
		{
			audio.SetLoader(loader);
		}
	}

	public void PopulateGroups() 
	{
		foreach(AudioFile file in files) 
		{
			foreach(AudioGroup group in groups) 
			{
				if(ArrayUtil.Contains(file.Groups, group.Name)) 
				{
					group.AddFile(file);
				}
			}
		}
	}

	public AudioFile GetAudioFile(AudioClip clip)
	{
		return clipToFileDictionary[clip];
	}

	public AudioType GetAudioType(AudioClip clip) 
	{
		return GetAudioFile(clip).Type;
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
		for(int i = 0; i < files.Length; i++) 
		{
			files[i].OnClipRequest += processAudioFileAccess;
		}
		AreEventsSubscribed = true;
	}

	void unsubscribeEvents() 
	{
		for(int i = 0; i < files.Length; i++) 
		{
			files[i].OnClipRequest -= processAudioFileAccess;
		}
	}

	void handleClipRequest(AudioFile file) 
	{
		processAudioFileAccess(file);
	}		

}
