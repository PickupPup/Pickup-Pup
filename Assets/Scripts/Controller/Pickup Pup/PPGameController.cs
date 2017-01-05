/*
 * Author: Isaiah Mann
 * Description: Game controller for Pickup Pup
 */

using System.IO;
using UnityEngine;
using System.Collections.Generic;

 public class PPGameController : GameController 
{
	const string JSON_DIR = "JSON";

	#region Static Accessors

	// Returns the Instance cast to the sublcass
	public static PPGameController GetInstance 
	{
		get 
		{
			return Instance as PPGameController;
		}
	}

	#endregion

	static string TUNING_FILE_PATH
	{
		get
		{
			return Path.Combine(JSON_DIR, "Tuning");
		}
	}
	static string GAME_DATA_FILE_PATH 
	{
		get 
		{
			return Path.Combine(JSON_DIR, "GameData");
		}
	}
	static string SAVE_FILE_PATH 
	{
		get 
		{
			return Path.Combine(Application.persistentDataPath, "PickupPupSave.dat");
		}
	}

	#region Instance Accesors

	public DogDatabase Data
	{
		get {
			return data; 
		}
	}

	public Currency Coins
	{
		get 
		{ 
			return save.Coins; 
		}
	}
	public Currency DogFood
	{
		get 
		{ 
			return save.DogFood; 
		}
	}
	public bool DogsScoutingAtCapacity 
	{
		get 
		{
			return dogsOutScouting.Count >= tuning.MaxDogsScouting;
		}
	}

	#endregion

	List<Dog> dogsOutScouting = new List<Dog>();
	PPTuning tuning;
	DogDatabase data;
	PPDataController save;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		data = parseDatabase();
		tuning = parseTuning();
		data.Initialize();
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		save = PPDataController.GetInstance;
		save.SetFilePath(SAVE_FILE_PATH);
		save.LoadGame();
	}
		
	#endregion

	public void ChangeCoins(int deltaCoins) 
	{
		save.ChangeCoins(deltaCoins);
	}

	public void ChangeFood(int deltaFood) 
	{
		save.ChangeFood(deltaFood);
	}
		
	public bool TrySendDogToScout(Dog dog) 
	{
		// Can only send a certain nubmer of dogs out to scout
		if(DogsScoutingAtCapacity || dogsOutScouting.Contains(dog)) 
		{
			return false;
		} 
		else 
		{
			sendDogToScout(dog);
			return true;
		}
	}

	void sendDogToScout(Dog dog) 
	{
		dogsOutScouting.Add(dog);
		dog.SubscribeToScoutingTimerEnd(handleDogDoneScouting);
	}

	void handleDogDoneScouting(Dog dog) 
	{
		dogsOutScouting.Remove(dog);
		// Need to unsubscribe to prevent stacking even subscriptions if dog is sent to scout again:
		dog.UnsubscribeFromScoutingTimerEnd(handleDogDoneScouting);
	}

	DogDatabase parseDatabase() 
	{
		TextAsset json = loadTextAssetInResources(GAME_DATA_FILE_PATH);
		return JsonUtility.FromJson<DogDatabase>(json.text);
	}

	PPTuning parseTuning() 
	{
		TextAsset json = loadTextAssetInResources(TUNING_FILE_PATH);
		return JsonUtility.FromJson<PPTuning>(json.text);
	}

}
