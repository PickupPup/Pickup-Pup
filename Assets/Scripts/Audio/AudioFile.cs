/*
 * Author(s): Isaiah Mann 
 * Description: Stores information about an audio file
 * Parsed from a JSON file
 * Events are stored as strings that indicate when to start and stop a file
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioFile : AudioData, IAudioFile 
{

	#region Instance Accessors

	#region IAudioFile Interface 

	public AudioClip Clip 
	{
		get 
		{
			if(_clip == null) {
				_clip = AudioLoader.GetClip(Name);
				callOnClipRequest();
			}
			return _clip;
		}
	}
	public AudioType TypeAsEnum 
	{
		get 
		{
			return AudioUtil.AudioTypeFromString(Type);
		}
	}
	public float ClipLength 
	{
		get {
			return Clip.length;
		}
	}
	// Volume for the AudioSource class uses 0-1.0f scale while our class uses 0-100 (integer) scale
	public float Volumef {
		get {
			return GetVolume();
		}
	}

	#endregion

	#endregion

	public bool Loop;
	public int Volume;
	public int Channel;
	public string[] Groups;
	public delegate void ClipRequestAction(AudioFile file);
	public event ClipRequestAction OnClipRequest;

	AudioClip _clip;

	#region AudioData Override

	public override AudioFile GetNextFile() 
	{
		return this;
	}

	public override AudioFile GetCurrentFile() 
	{
		return this;
	}

	#endregion

	#region System.Object Overrides

	public override string ToString() 
	{
		return string.Format (
			"[AudioFile:\n"+
			"FileName={0}\n" +
			"EventNames={1}\n" +
			"EndEventNames={2}\n" +
			"Loop={3}\n" +
			"Type={4}\n" +
			"Channel={5}" +
			"]", 
			Name, 
			ArrayUtil.ToString(Events),
			ArrayUtil.ToString(StopEvents),
			Loop, 
			Type, 
			Channel);
	}

	#endregion

	#region IAudioFile Interface

	public bool HasEvent(string eventName) 
	{
		return ArrayUtil.Contains (Events, eventName);
	}

	public bool HasEndEvent(string eventName) 
	{
		return ArrayUtil.Contains (StopEvents, eventName);
	}
		
	public float GetVolume() 
	{
		return getVolume(Volume);
	}

	#endregion

	void callOnClipRequest() 
	{
		if(OnClipRequest != null) 
		{ 
			OnClipRequest(this);
		}
	}

	float getVolume(int volume) 
	{
		return (float) volume / 100f;
	}


	public void SetClip(AudioClip clip) 
	{
		this._clip = clip;
		callOnClipRequest();
	}
		
	public bool ClipIsSet()
	{
		return _clip != null;
	}

}
