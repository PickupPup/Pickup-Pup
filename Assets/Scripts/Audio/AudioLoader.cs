/*
 * Author: Isaiah Mann
 * Description: Class to load in the audio from a JSON file
 */

using System.IO;
using System.Collections;
using UnityEngine;

public class AudioLoader : ResourceLoader
{
	#region Static Accessors

	public static AudioLoader Default
	{
		get 
		{
			return new AudioLoader(defaultJSONPath, AUDIO_DIR);
		}
	}

	#endregion

	static string defaultJSONPath
	{
		get
		{
			return Path.Combine(JSON_DIR, "AudioList");
		}
	}


	// The path within the directory where the JSON file is saved
	string jsonPath;
	string audioPath;

	public AudioLoader(string jsonPath, string audioPath) 
	{
		this.jsonPath = jsonPath;
		this.audioPath = audioPath;
	}

	// Returns a C# class formatted like corresponding JSON file
	// JSON file must be formatted to match class structure or will throw an error
	public AudioList Load() 
	{
		string jsonText = FileUtil.FileText(this.jsonPath);
		AudioList list = JsonUtility.FromJson<AudioList>(jsonText);
		list.SetLoader(this);
		return list;
	}
		
	// Fetches a particular clip from the resources folder
	public AudioClip GetClip(string fileName) 
	{
		return Resources.Load<AudioClip>(Path.Combine(audioPath, fileName));
	}

	public ResourceRequest GetClipAsync(string fileName) 
	{
		return Resources.LoadAsync<AudioClip>(Path.Combine(audioPath, fileName));
	}

	public AudioClip GetClip(AudioFile file) 
	{
		return GetClip(file.Name);
	}

}
