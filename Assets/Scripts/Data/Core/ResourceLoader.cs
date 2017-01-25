/*
 * Author: Isaiah Mann
 * Description: Generic superclass to handle loading from resources
 */

using k = PPGlobal;
using UnityEngine;
using SimpleJSON;
using System.IO;

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

	protected const char JOIN_CHAR = k.JOIN_CHAR;

	protected T loadFromResources<T>(string path) where T : UnityEngine.Object
	{
		return Resources.Load<T>(path);
	}

	protected string getTextFromResources(string path)
	{
		return loadFromResources<TextAsset>(path).text;
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
