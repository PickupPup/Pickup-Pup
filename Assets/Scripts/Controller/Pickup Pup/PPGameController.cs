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
	static string SAVE_FILE_PATH {
		get {
			return Path.Combine(Application.persistentDataPath, "PickupPupSave.dat");
		}
	}

	DogDatabase data;
	PPDataController save;

	protected override void SetReferences () {
		base.SetReferences ();
		data = JsonUtility.FromJson<DogDatabase>(Resources.Load<TextAsset>(GAME_DATA_FILE_PATH).text);
		data.Initialize();
	}

	protected override void FetchReferences () {
		base.FetchReferences ();
		save = PPDataController.GetInstance;
		save.SetFilePath(SAVE_FILE_PATH);
		save.LoadGame();
	}

    public DogDatabase Data
    {
        get { return data; }
    }

    public Currency Coins
    {
		get { return save.Coins; }
    }

    public Currency DogFood
    {
		get { return save.DogFood; }
    }

	public void ChangeCoins (int deltaCoins) {
		save.ChangeCoins(deltaCoins);
	}

	public void ChangeFood (int deltaFood) {
		save.ChangeFood(deltaFood);
	}
}