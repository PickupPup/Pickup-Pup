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

	public bool Loop 
	{
		get 
		{
			return this.loop;
		}
	}
		
	public string[] Groups
	{
		get
		{
			return this.groups;
		}
	}

	public int Channel
	{
		get 
		{
			return this.channel;
		}
	}

	public AudioClip Clip 
	{
		get 
		{
			if(_clip == null) {
				_clip = loader.GetClip(this);
				callOnClipRequest();
			}
			return _clip;
		}
	}

	public AudioType Type 
	{
		get 
		{
			return AudioUtil.AudioTypeFromString(type);
		}
	}

	public float ClipLength 
	{
		get 
		{
			return Clip.length;
		}
	}

	// Volume for the AudioSource class uses 0-1.0f scale while our class uses 0-100 (integer) scale
	public float Volumef 
	{
		get
		{
			return GetVolume();
		}
	}

	#endregion

	#endregion

	public delegate void ClipRequestAction(AudioFile file);
	public event ClipRequestAction OnClipRequest;

	[SerializeField]
	bool loop;
	[SerializeField]
	int volume;
	[SerializeField]
	int channel;
	[SerializeField]
	string[] groups;

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
		return string.Format(
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
			loop, 
			type, 
			channel);
	}

	#endregion

	#region IAudioFile Interface

	public bool HasEvent(string eventName) 
	{
		return ArrayUtil.Contains(Events, eventName);
	}

	public bool HasEndEvent(string eventName) 
	{
		return ArrayUtil.Contains(StopEvents, eventName);
	}
		
	public float GetVolume() 
	{
		return percentToDecimal(volume);
	}

	#endregion

	void callOnClipRequest() 
	{
		if(OnClipRequest != null) 
		{ 
			OnClipRequest(this);
		}
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
