/*
 * Author: Isaiah Mann
 * Description: Generic superclass to handle loading from resources
 */

using k = PPGlobal;

[System.Serializable]
public class ResourceLoader 
{
	protected const string AUDIO_DIR = k.AUDIO_DIR;
	protected const string JSON_DIR = k.JSON_DIR;
	protected const string SPRITES_DIR = k.SPRITES_DIR;
	protected const string GIFTS_DIR = k.GIFTS_DIR;
	protected const string DEFAULT = k.DEFAULT;
	protected const string GAME_DATA = k.GAME_DATA;
	protected const string EXPORT = k.EXPORT;
	protected const string SHEET = k.SHEET;
	protected const string TUNING = k.TUNING;

	protected const char JOIN_CHAR = k.JOIN_CHAR;

    const float FULL_PERCENT = k.FULL_PERCENT_F;

    protected static float perecentToDecimal(int percentOf100)
    {
        return ((float) percentOf100) / FULL_PERCENT;
    }

    protected static int decimalToPercent(float fraction)
    {
        return (int) (fraction * FULL_PERCENT);
    }

}
