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
            return _showedShelterPrompt;
        }
        set
        {
            _showedShelterPrompt = value;
        }
    }

    public static bool ShowedScoutingPrompt
    {
        get
        {
            return _showedScoutingPrompt;
        }
        set
        {
            _showedScoutingPrompt = value;
        }
    }

	public static bool ShowedFirstLivingRoomPrompt
	{
		get
		{
			return _showedFirstLivingRoomPrompt;
		}
		set
		{
			_showedFirstLivingRoomPrompt = value;
		}
	}

	public static bool ShowedShopPrompt
	{
		get
		{
			return _showedShopPrompt;
		}
		set
		{
			_showedShopPrompt = value;
		}
	}

	public static bool CompletedLivingRoomTutorial
	{
		get
		{
			return _completedLivingRoomTutorial;	
		}
		set
		{
			_completedLivingRoomTutorial = value;
		}
	}

	public static bool CompletedShelterTutorial
	{
		get
		{
			return _completedShelterTutorial;
		}
		set
		{
			_completedShelterTutorial = value;
		}
	}

	public static bool CompletedShopTutorial
	{
		get
		{
			return _completedShopTutorial;
		}
		set
		{
			_completedShopTutorial = value;
		}
	}

	#endregion

	static bool _showedShelterPrompt;
	static bool _showedScoutingPrompt;
	static bool _showedFirstLivingRoomPrompt;
	static bool _showedShopPrompt;

	static bool _completedLivingRoomTutorial;
	static bool _completedShelterTutorial;
	static bool _completedShopTutorial;

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
