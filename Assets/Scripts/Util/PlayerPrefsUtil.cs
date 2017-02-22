/*
* Authors: Isaiah Mann, Grace Barrett-Snyder
* Description: Util class to complement Unity's PlayerPrefs class
*/

using UnityEngine;

public static class PlayerPrefsUtil 
{
	#region Static Accessors 

    public static bool ShowedShelterPrompt
    {
        get
        {
            return showedShelterPrompt;
        }
        set
        {
            showedShelterPrompt = value;
        }
    }

    public static bool ShowedScoutingPrompt
    {
        get
        {
            return showedScoutingPrompt;
        }
        set
        {
            showedScoutingPrompt = value;
        }
    }

	public static bool ShowedFirstLivingRoomPrompt
	{
		get
		{
			return showedFirstLivingRoomPrompt;
		}
		set
		{
			showedFirstLivingRoomPrompt = value;
		}
	}

	public static bool ShowedShopPrompt
	{
		get
		{
			return showedShopPrompt;
		}
		set
		{
			showedShopPrompt = value;
		}
	}

	#endregion

    static bool showedShelterPrompt;
    static bool showedScoutingPrompt;
	static bool showedFirstLivingRoomPrompt;
	static bool showedShopPrompt;

	/*
	 * PlayerPrefs has no bool class
	 * This wrapped provides that functionality
	*/
	public static bool GetBool(string key) 
	{
		return IntToBool(PlayerPrefs.GetInt(key, 0));
	}

	public static void SetBool(string key, bool value) 
	{
		PlayerPrefs.SetInt(key, BoolToInt(value));
	}

	static bool IntToBool(int value) 
	{
		return !(value == 0);
	}

	static int BoolToInt(bool value) 
	{
		return value ? 1 : 0;
	}

}
