/*
 * Author(s): Isaiah Mann
 * Description: Used for loading in Spritesheets from Resources
 * Usage: Requires a JSON sheet to format
 */

using System.IO;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;
using k = PPGlobal;

[System.Serializable]
public class SpritesheetDatabase : Database<SpritesheetDatabase>
{
	#region Static Accessors

	public static SpritesheetDatabase GetInstance
	{
		get
		{
			SpritesheetDatabase database = Instance;
			// Initializes the database if it's not already setup
			database.TryInit();
			return database;
		}
	}

	#endregion

	static string jsonPath 
	{
		get
		{
			return Path.Combine(k.JSON_DIR, k.SPRITES_DIR);
		}
	}

	[System.NonSerialized]
	Dictionary<string, Sprite> spriteLookup;

	public override void Initialize()
	{
		base.Initialize();
		TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
		JSONNode json = JSON.Parse(jsonFile.text);
		spriteLookup = initSpriteLookup(json[k.EXPORT]);
	}

	public bool TryGetSprite(string spriteName, out Sprite sprite)
	{
		checkSpriteBuffer();
		return spriteLookup.TryGetValue(spriteName, out sprite);
	}
		
	Dictionary<string, Sprite> initSpriteLookup(JSONNode json)
	{
		Dictionary<string, Sprite> lookup = new Dictionary<string, Sprite>();
		foreach(JSONNode sheet in json.Childs)
		{
			string sheetName = sheet[k.SHEET];
			Sprite[] spriteSheet = Resources.LoadAll<Sprite>(Path.Combine(k.SPRITES_DIR, sheetName));
			int index = 0;
			foreach(JSONNode sprite in sheet[k.SPRITES_DIR].AsArray)
			{
				lookup.Add(sprite, spriteSheet[index++]);
			}
		}
		return lookup;
	}

	void checkSpriteBuffer()
	{
		if(spriteLookup == null)
		{
			spriteLookup = new Dictionary<string, Sprite>();
		}
	}

}
