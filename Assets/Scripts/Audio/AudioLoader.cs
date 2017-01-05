/*
 * Author: Isaiah Mann
 * Description: Class to load in the audio from a JSON file
 * Dependencies: None
 */
using UnityEngine;
using System.Collections;

public class AudioLoader 
{

	const string DIRECTORY = "Audio/";

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
		return Resources.Load<AudioClip>(DIRECTORY + fileName);
	}

	public static ResourceRequest GetClipAsync(string fileName) 
	{
		return Resources.LoadAsync<AudioClip>(DIRECTORY + fileName);
	}

	public static AudioClip GetClip(AudioFile file) 
	{
		return GetClip(file.Name);
	}

}
