/*
 * Author(s): Isaiah Mann
 * Description: Holds a summary of data supported languages
 * Usage: Intended to support Localization work
 */

using SimpleJSON;
using System.Collections.Generic;

[System.Serializable]
public class LanguageDatabase : Database<LanguageDatabase>
{
	string[] wordKeys;

	public override void Initialize()
	{
		base.Initialize();
		JSONNode json = getJSONFromResources(LANGUAGES);
	}

	void populateLanguages(JSONNode json)
	{
		// JSONNode supportedJSON = json[
	}

}

[System.Serializable]
public class Language : SerializableData
{
	#region Instance Accessors

	public string Name 
	{
		get 
		{
			return languageName;
		}
	}

	#endregion

	string languageName;
	Dictionary<string, string> wordLookup;

	public Language(string languageName, ParallelArray<string, string> terms)
	{
		this.languageName = languageName;
		this.wordLookup = terms.ToDict();
	}
		
	public string Get(string key) 
	{
		string value;
		if(wordLookup.TryGetValue(key, out value))
		{
			return value;
		}
		else 
		{
			UnityEngine.Debug.LogErrorFormat("Key {0} not found in language", key);
			return string.Empty;
		}
	}
		
}
