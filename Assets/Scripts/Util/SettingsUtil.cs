/*
* Author: Isaiah Mann
* Description: Util class for simple player settings
*/
using UnityEngine;
using System.Collections;
using k = Global;

public static class SettingsUtil 
{	
	// Keys used to acccess the settings from player prefs
	const string musicMuteSettingsKey = k.MUSIC_MUTE;
	const string fxMuteSettingsKey = k.FX_MUTE;
	const string voMuteSettingsKey = k.VO_MUTE;

	#region Static Accessors

	public static bool MusicMuted 
	{
		get 
		{
			return IsMuted(musicMuteSettingsKey);
		}
	}

	public static bool SFXMuted 
	{
		get 
		{
			return IsMuted(fxMuteSettingsKey);
		}
	}

	public static bool VOMuted 
	{
		get 
		{
			return IsMuted(voMuteSettingsKey);
		}
	}

	#endregion

	public static void ToggleMusicMuted(bool muted) 
	{
		ToggleMute(musicMuteSettingsKey, muted);
		EventController.Event 
		(
			AudioUtil.MuteActionFromBool(muted),
			AudioType.Music
		);
	}

	public static void ToggleMusicMuted() 
	{
		ToggleMusicMuted(!MusicMuted);
	}

	public static void ToggleSFXMuted(bool muted) 
	{
		ToggleMute(fxMuteSettingsKey, muted);
		EventController.Event 
		(
			AudioUtil.MuteActionFromBool(muted),
			AudioType.FX
		);
	}

	public static void ToggleSFXMuted() 
	{
		ToggleSFXMuted(!SFXMuted);
	}

	public static void ToggleVOMuted(bool muted) 
	{
		ToggleMute(voMuteSettingsKey, muted);
		EventController.Event 
		(
			AudioUtil.MuteActionFromBool(muted),
			AudioType.VO
		);
	}

	public static void ToggleVOMuted() 
	{
		ToggleVOMuted(!VOMuted);
	}


	static void ToggleMute(string key, bool value) 
	{
		PlayerPrefsUtil.SetBool(key, value);
	}

	static bool IsMuted(string key) 
	{
		return PlayerPrefsUtil.GetBool(key);
	}

}
