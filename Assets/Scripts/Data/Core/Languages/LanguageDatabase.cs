/*
 * Author(s): Isaiah Mann
 * Description: Holds a summary of data supported languages
 * Usage: Intended to support Localization work
 */

using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class LanguageDatabase : Database<LanguageDatabase>
{
	#region Instance Accessors

	public string this[string key]
	{
		get
		{
			if(HasCurrentLanguage)
			{
				return currentLanguage[key];
			}
			else 
			{
				return string.Empty;
			}
		}
	}
		
	public bool HasCurrentLanguage 
	{
		get 
		{
			return currentLanguage != null;
		}
	}
		
	#endregion

	string[] wordKeys;
	Language currentLanguage;
	Dictionary<string, Language> languages;

	public override void Initialize()
	{
		base.Initialize();
		JSONNode json = getJSONFromResources(LANGUAGES);
		this.languages = populateLanguages(json);
		Language[] options = this.languages.Values.ToArray();
		if(!trySetDefaultLanguage(options))
		{
			currentLanguage = options[default(int)];
		}
		AssignInstance(this);
	}

	public bool HasTerm(string key)
	{
		return currentLanguage.HasTerm(key);
	}

	public string GetTerm(string key)
	{
		return currentLanguage.GetTerm(key);
	}

	public bool TrySetLanguage(string languageName)
	{
		Language newLanguage;
		if(this.languages.TryGetValue(languageName, out newLanguage))
		{
			this.currentLanguage = newLanguage;
			return true;
		}
		else 
		{
			return false;
		}
	}

	public string[] GetSupportedLanguages()
	{
		return languages.Keys.ToArray();
	}

	bool trySetDefaultLanguage(Language[] options)
	{
		bool languageSet = false;
		for(int i = 0; i < options.Length; i++)
		{
			if(options[i].IsDefault)
			{
				this.currentLanguage = options[i];
				languageSet = true;
				break;
			}
		}
		return languageSet;
	}

	Dictionary<string, Language> populateLanguages(JSONNode json)
	{
		int keyOffset = 1;
		int currentLanguageIndex = keyOffset;
		Dictionary<string, int> languageIndexes = new Dictionary<string, int>();
		JSONArray supportedLanguages = json[SUPPORTED_LANGUAGES].AsArray;
		int languageCount = supportedLanguages.Count;
		bool[] defaultLanguageRecord = new bool[languageCount];
		foreach(JSONNode language in supportedLanguages)
		{
			if(language[SUPPORTED].AsBool)
			{
				languageIndexes.Add(language[LANGUAGE_NAME], currentLanguageIndex);
				defaultLanguageRecord[currentLanguageIndex - keyOffset] = language[DEFAULT].AsBool;
				currentLanguageIndex++;
			}
		}
		JSONArray lookup = json[LOOKUP].AsArray;
		// First array in the jagged array stores keys
		int keysIndex = 0;
		string[][] values = getTerms(lookup, 
			keysIndex, currentLanguageIndex, languageIndexes);
		Dictionary<string, Language> languagesByName = new Dictionary<string, Language>();
		foreach(string languageName in languageIndexes.Keys)
		{
			int languageIndex = languageIndexes[languageName];
			Language language = createLanguage(
				languageName, 
				keysIndex, 
				languageIndex,
				values,
				isSupported:true,
				isDefault:defaultLanguageRecord[languageIndex - keyOffset]);
			languagesByName.Add(languageName, language);
		}
		return languagesByName;
	}

	string[][] getTerms(JSONArray lookup, 
		int keysIndex, 
		int numLanguages,
		Dictionary<string, int> languageIndexes)
	{
		int keyCount = lookup.Count;
		string[][] values = new string[numLanguages][];
		for(int i = 0; i < values.Length; i++)
		{
			values[i] = new string[keyCount];
		}
		int currentTermIndex = 0;
		foreach(JSONNode term in lookup)
		{
			values[keysIndex][currentTermIndex] = term[KEY];
			foreach(string language in languageIndexes.Keys)
			{
				values[languageIndexes[language]][currentTermIndex] = term[language];
			}
			currentTermIndex++;
		}
		return values;
	}

	Language createLanguage(string languageName, 
		int keyIndex,
		int termIndex,
		string[][] terms,
		bool isSupported,
		bool isDefault)
	{
		ParallelArray<string, string> lookup = new ParallelArray<string, string>(
			terms[keyIndex], terms[termIndex]);
		return new Language(languageName, isSupported, isDefault, lookup);
	}

}

[System.Serializable]
public class Language : SerializableData
{
	#region Instance Accessors

	public string this[string key]
	{
		get
		{
			return GetTerm(key);
		}
	}
		
	public string Name 
	{
		get 
		{
			return languageName;
		}
	}

	public bool IsSupported 
	{
		get 
		{
			return isSupported;
		}
	}

	public bool IsDefault
	{
		get 
		{
			return isDefault;
		}
	}

	#endregion

	string languageName;
	bool isSupported;
	bool isDefault;

	Dictionary<string, string> wordLookup;

	public Language(string languageName, 
		bool isSupported, 
		bool isDefault, 
		ParallelArray<string, string> terms)
	{
		this.languageName = languageName;
		this.isSupported = isSupported;
		this.isDefault = isDefault;
		this.wordLookup = terms.ToDict();
	}
		
	public string GetTerm(string key) 
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
		
	public bool HasTerm(string key)
	{
		return this.wordLookup.ContainsKey(key);
	}

}
