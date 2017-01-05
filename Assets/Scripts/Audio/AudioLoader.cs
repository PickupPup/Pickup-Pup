/*
 * Author: Isaiah Mann
 * Description: Class to load in the audio from a JSON file
 */

using UnityEngine;
using System.Collections;

public class AudioLoader 
{
	const string DIR = "Audio/";

	// The path within the directory where the JSON file is saved
	string path;

	public AudioLoader(string path) 
	{
		this.path = path;
	}

	// Returns a C# class formatted like corresponding JSON file
	// JSON file must be formatted to match class structure or will throw an error
	public AudioList Load() 
	{
		string jsonText = FileUtil.FileText(this.path);
		return JsonUtility.FromJson<AudioList>(jsonText);
	}
		
	// Fetches a particular clip from the resources folder
	public static AudioClip GetClip(string fileName) 
	{
		return Resources.Load<AudioClip>(DIR + fileName);
	}

	public static ResourceRequest GetClipAsync(string fileName) 
	{
		return Resources.LoadAsync<AudioClip>(DIR + fileName);
	}

	public static AudioClip GetClip(AudioFile file) 
	{
		return GetClip(file.Name);
	}

}
