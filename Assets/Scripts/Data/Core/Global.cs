/*
 * Author(s): Isaiah Mann
 * Description: Stores global data
 * Usage: [no notes]
 */

using System;

public class Global
{
	public const string AUDIO_DIR = "Audio";
	public const string JSON_DIR = "JSON";
	public const string SPRITES_DIR = "Sprites";
	public const string GIFTS_DIR = "Gifts";
	public const string DEFAULT = "Default";
	public const string GAME_DATA = "GameData";
	public const string EXPORT = "Export";
	public const string SHEET = "Sheet";
	public const string TUNING = "Tuning";
	public const string HEX_HASH_PREFIX = "#";
	public const string HEX_NUM_PREFIX = "0x";
	public const string BLACK_HEX = "#000000";
	public const string MUSIC_MUTE = "musicMute";
	public const string FX_MUTE = "fxMute";
	public const string VO_MUTE = "voMute";
	public const string LANGUAGES = "Languages";
	public const string LOOKUP = "Lookup";
	public const string SUPPORTED = "Supported";
	public const string SUPPORTED_LANGUAGES = SUPPORTED + LANGUAGES;
	public const string KEY = "Key";
	public const string LANGUAGE = "Language";
	public const string NAME = "Name";
	public const string LANGUAGE_NAME = LANGUAGE + NAME;
	public const string OFF = "Off";
	public const string ON = "On";
	public const string VOLUME = "Volume";
	public const string MUSIC = "Music";
	public const string MUSIC_VOLUME = MUSIC + VOLUME;
    public const string STANDARD = "Standard";
    public const string ALTERNATE = "Alternate";
    public const string SECONDS = "Seconds";
    public const string TYPE = "Type";
    public const string AMOUNT = "Amount";
    public const string SESSION_COUNT = "Game Session Count";
    public const string TIME_PLAYED = "Total Time Played";

	public const char JOIN_CHAR = '_';

	public const int FULL_VOLUME = 100;
	public const int CORRECT_HEX_NUM_LENGTH = 6;
	public const int NONE_VALUE = 0;
	public const int INVALID_VALUE = -1;
	public const int SINGLE_VALUE = 1;
	public const int TRUE_VALUE_INT = 1;
	public const int FALSE_VALUE_INT = NONE_VALUE;
    public const int ZERO_INDEX_OFFSET = 1;

    public const float FULL_PERCENT_F = 100f;
    public const float FULL_PERCENT_DECIMAL = 1.0f;
    public const float MIN_SWIPE_THRESHOLD = 1f;
    public const float DEFAULT_TIME_SCALE = 1.0f;

	public static bool IntToBool(int value)
	{
		switch(value)
		{
			case TRUE_VALUE_INT:
				return true;
			case FALSE_VALUE_INT:
				return false;
			default:
				return value > FALSE_VALUE_INT;
		}
	}

	public static int BoolToInt(bool value)
	{
		switch(value)
		{
			case true:
				return TRUE_VALUE_INT;
			case false:
				return FALSE_VALUE_INT;
			default:
				return default(int);	
		}
	}

    public static bool IsPositive(float value)
    {
        return Math.Sign(value) > NONE_VALUE;
    }

}
