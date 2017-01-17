/*
 * Author: Isaiah Mann
 * Description: Class to load in the audio from a JSON file
 */

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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

    List<string> fileNames = new List<string>();   

	public AudioLoader(string jsonPath, string audioPath) 
	{
		this.jsonPath = jsonPath;
		this.audioPath = audioPath;

        loadAudioFilenames();
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

    void loadAudioFilenames()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(AUDIO_DIR);
        foreach(AudioClip clip in audioClips)
        {
            if (!fileNames.Contains(clip.name))
            {
                fileNames.Add(clip.name);
            }
            Resources.UnloadAsset(clip);
        }
    }

    // Gets the full name of the Audio File
    // Ex: "MainMusic" will become "PickupPup_Music_MainMusic_V#"
    // Where # is the version number
    string getFullAudioFileName(AudioFile file)
    {
        Regex rgx = new Regex(file.Name);
        foreach(string fullFileName in fileNames)
        {
            if (rgx.IsMatch(fullFileName))
            {
                return fullFileName;
            }
        }
        throw new KeyNotFoundException(file.Name);
    }

    // Fetches a particular clip from the resources folder
    public AudioClip GetClip(string fullFileName) 
	{
        return Resources.Load<AudioClip>(Path.Combine(audioPath, fullFileName));
	}

	public ResourceRequest GetClipAsync(AudioFile file) 
	{
        string fullFileName = getFullAudioFileName(file);
        return Resources.LoadAsync<AudioClip>(Path.Combine(audioPath, fullFileName));
	}

	public AudioClip GetClip(AudioFile file) 
	{
		return GetClip(getFullAudioFileName(file));
	}

}
