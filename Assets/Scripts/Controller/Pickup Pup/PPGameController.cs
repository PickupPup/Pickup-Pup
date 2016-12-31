/*
 * Author: Isaiah Mann
 * Description: Game controller for Pickup Pup
 */

using System.IO;
using UnityEngine;

public class PPGameController : GameController {

	const string JSON_DIR = "JSON";

	static string GAME_DATA_FILE_PATH {
		get {
			return Path.Combine(JSON_DIR, "GameData");
		}
	}

	DogDatabase data;
    Currency coins;
    Currency dogFood;

	protected override void SetReferences () {
		base.SetReferences ();
		data = JsonUtility.FromJson<DogDatabase>(Resources.Load<TextAsset>(GAME_DATA_FILE_PATH).text);
		data.Initialize();

        coins = new Currency(CurrencyType.Coins, 100);
        dogFood = new Currency(CurrencyType.DogFood, 50);
	}

    public DogDatabase Data
    {
        get { return data; }
    }

    public Currency Coins
    {
        get { return coins; }
    }

    public Currency DogFood
    {
        get { return dogFood; }
    }
}