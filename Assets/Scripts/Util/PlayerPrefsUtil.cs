/*
* Author: Isaiah Mann
* Description: Util class to complement Unity's PlayerPrefs class
*/

using UnityEngine;

public static class PlayerPrefsUtil 
{
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

    public static bool CompletedShelterTutorial
    {
        get
        {
            return completedShelterTutorial;
        }
        set
        {
            completedShelterTutorial = value;
        }
    }

    public static bool CompletedLivingRoomTutorial
    {
        get
        {
            return completedLivingRoomTutorial;
        }
        set
        {
            completedLivingRoomTutorial = value;
        }
    }

    public static bool CompletedShopTutorial
    {
        get
        {
            return completedShopTutorial;
        }
        set
        {
            completedShopTutorial = value;
        }
    }

    static bool showedShelterPrompt;
    static bool showedScoutingPrompt;
    static bool completedShelterTutorial;
    static bool completedLivingRoomTutorial;
    static bool completedShopTutorial;

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
