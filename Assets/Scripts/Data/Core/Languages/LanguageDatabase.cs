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
		// TODO: Parse paralell array to dict:
	}
		
}
