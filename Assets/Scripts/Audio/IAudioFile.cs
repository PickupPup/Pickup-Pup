/*
 * Author(s): Isaiah Mann
 * Description: Describes the intended behaviour of an audio file
 */

using UnityEngine;

public interface IAudioFile 
{
	#region Instance Accessors

	AudioClip Clip 
	{
		get;
	}

	float ClipLength 
	{
		get;
	}

	// Volume for the AudioSource class uses 0-1.0f scale while our class uses 0-100 (integer) scale
	float Volumef 
	{
		get;
	}

	AudioType Type 
	{
		get;
	}
		
	#endregion

	bool HasEvent(string eventName);
	bool HasEndEvent(string eventName);

	float GetVolume();

}
