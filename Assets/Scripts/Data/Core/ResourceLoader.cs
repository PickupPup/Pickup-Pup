/*
 * Author: Isaiah Mann
 * Description: Generic superclass to handle loading from resources
 */

using k = PPGlobal;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
	protected const string LANGUAGES = k.LANGUAGES;
	protected const string SUPPORTED_LANGUAGES = k.SUPPORTED_LANGUAGES;
	protected const string LANGUAGE_NAME = k.LANGUAGE_NAME;
	protected const string SUPPORTED = k.SUPPORTED;
	protected const string LOOKUP = k.LOOKUP;
	protected const string KEY = k.KEY;
    protected const string SOUVENIRS = k.SOUVENIRS;

	protected const char JOIN_CHAR = k.JOIN_CHAR;

	protected const float DEFAULT_DISCOUNT = k.DEFAULT_DISCOUNT_DECIMAL;

    protected const int ZERO_INDEX_OFFSET = k.ZERO_INDEX_OFFSET;

	const float FULL_PERCENT = k.FULL_PERCENT_F;

    // Adapted from: http://stackoverflow.com/questions/1031023/copy-a-class-c-sharp
    // Returns null if cast is invalid
    public T Copy<T>() where T : class
    { 
        using(MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, this);
            memoryStream.Position = 0;
            return formatter.Deserialize(memoryStream) as T;
        }
    }

    protected static float perecentToDecimal(int percentOf100)
    {
        return ((float) percentOf100) / FULL_PERCENT;
    }

    protected static int decimalToPercent(float fraction)
    {
        return (int) (fraction * FULL_PERCENT);
    }

	protected T loadFromResources<T>(string path) where T : UnityEngine.Object
	{
		return Resources.Load<T>(path);
	}

	protected string getTextFromResources(string path)
	{
		return loadFromResources<TextAsset>(path).text;
	}

    protected void overwriteFromJSONInResources<T>(string fileName, T target)
    {
        string json = Resources.Load<TextAsset>(getJSONPathInResources(fileName)).text;
        JsonUtility.FromJsonOverwrite(json, target);
    }

	protected JSONNode getJSONFromResources(string fileName)
	{
		string jsonText = getTextFromResources(getJSONPathInResources(fileName));
		return JSON.Parse(jsonText);
	}

	protected string getJSONPathInResources(string fileName)
	{
		return Path.Combine(JSON_DIR, fileName);
	}

}
