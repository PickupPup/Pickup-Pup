/*
 * Author(s): Isaiah Mann 
 * Description: Stores information about an audio file
 * Parsed from a JSON file
 * Events are stored as strings that indicate when to start and stop a file
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioFile : AudioData, IAudioFile {
	public delegate void ClipRequestAction (AudioFile file);
	public event ClipRequestAction OnClipRequest;
	public string[] Groups;
	AudioClip _clip;
	public AudioClip Clip {
		get {
			if (_clip == null) {
				_clip = AudioLoader.GetClip(Name);
				CallOnClipRequest();
			}

			return _clip;
		}
	}

	public override AudioFile GetNextFile () {
		return this;
	}

	public override AudioFile GetCurrentFile () {
		return this;
	}

	public float ClipLength {
		get {
			return Clip.length;
		}
	}
		
	public bool Loop;
	public int Volume;

	// Volume for the AudioSource class uses 0-1.0f scale while our class uses 0-100 (integer) scale
	public float Volumef {
		get {
			return GetVolume();
		}
	}

	public AudioType TypeAsEnum {
		get {
			return AudioUtil.AudioTypeFromString(Type);
		}
	}

	public int Channel;

	public override string ToString () {
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

	public bool HasEvent (string eventName) {
		return ArrayUtil.Contains (
			Events,
			eventName
		);
	}

	public bool HasEndEvent (string eventName) {
		return ArrayUtil.Contains (
			StopEvents,
			eventName
		);
	}

	void CallOnClipRequest () {
		if (OnClipRequest != null) { 
			OnClipRequest(this);
		}
	}

	float GetVolume (int volume) {
		return (float)volume/100f;
	}

	public float GetVolume () {
		return GetVolume(Volume);
	}

	public void SetClip (AudioClip clip) {
		this._clip = clip;
		CallOnClipRequest();
	}
		
	public bool ClipIsSet () {
		return _clip != null;
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