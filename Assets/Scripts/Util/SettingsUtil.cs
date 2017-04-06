/*
* Author: Isaiah Mann
* Description: Util class for simple player settings
*/
using UnityEngine;
using System.Collections;

using k = PPGlobal;
using me = MonoBehaviourExtended;

public static class SettingsUtil 
{	
	// Keys used to acccess the settings from player prefs
	const string MUSIC_MUTE = k.MUSIC_MUTE;
	const string FX_MUTE = k.FX_MUTE;
	const string VO_MUTE = k.VO_MUTE;
	const string MUSIC_VOLUME = k.MUSIC_VOLUME;
    const string NAV_DROPDOWN_TYPE = k.NAV_DROPDOWN;

	const int DEFAULT_VOLUME = k.FULL_VOLUME;

    static me.MonoAction onNavPanelChanged;

	#region Static Accessors

	public static bool MusicMuted 
	{
		get 
		{
			return IsMuted(MUSIC_MUTE);
		}
	}

	public static bool SFXMuted 
	{
		get 
		{
			return IsMuted(FX_MUTE);
		}
	}

	public static bool VOMuted 
	{
		get 
		{
			return IsMuted(VO_MUTE);
		}
	}

    public static int NavDropDownType
    {
        get
        {
            return PlayerPrefs.GetInt(NAV_DROPDOWN_TYPE);
        }
        private set
        {
            int previousValue = NavDropDownType;
            PlayerPrefs.SetInt(NAV_DROPDOWN_TYPE, value);
            if(value != previousValue)
            {
                callOnNavPanelChanged();
            }
        }
    }

	#endregion

    public static void ToggleNavDropdownType()
    {
        if(NavDropDownType == k.STANDARD_DROPDOWN)
        {
            NavDropDownType = k.ALT_SINGLE_DROPDOWN;
        }
        else
        {
            NavDropDownType = k.STANDARD_DROPDOWN;
        }
    }

	public static int GetMusicVolume()
	{
		return PlayerPrefs.GetInt(MUSIC_VOLUME, DEFAULT_VOLUME);
	}

	public static void SetMusicVolume(int volume)
	{
		PlayerPrefs.SetInt(MUSIC_VOLUME, volume);
	}

	public static void ToggleMusicMuted(bool muted) 
	{
		ToggleMute(MUSIC_MUTE, muted);
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
		ToggleMute(FX_MUTE, muted);
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
		ToggleMute(VO_MUTE, muted);
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
     
    public static void SubscribeToNavPanelChange(me.MonoAction handler)
    {
        onNavPanelChanged += handler;
    }

    public static void UnsubscribeFromNavPanelChange(me.MonoAction handler)
    {
        onNavPanelChanged -= handler;
    }

	static void ToggleMute(string key, bool value) 
	{
		PlayerPrefsUtil.SetBool(key, value);
	}

	static bool IsMuted(string key) 
	{
		return PlayerPrefsUtil.GetBool(key);
	}

    static void callOnNavPanelChanged()
    {
        if(onNavPanelChanged != null)
        {
            onNavPanelChanged();
        }
    }

}
