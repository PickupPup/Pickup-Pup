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
		get 
		{
			return database; 
		}
	}

	public Currency Coins
	{
		get 
		{ 
			return dataController.Coins; 
		}
	}

	public Currency DogFood
	{
		get 
		{ 
			return dataController.DogFood; 
		}
	}

    public Currency VacantHomeSlots
    {
        get
        {
            return dataController.VacantHomeSlots;
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
	DogDatabase database;
	PPDataController dataController;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		database = parseDatabase();
		tuning = parseTuning();
		database.Initialize();
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
		dataController.SetFilePath(SAVE_FILE_PATH);
		dataController.LoadGame();
	}
		
	#endregion

	public void ChangeCoins(int deltaCoins) 
	{
		dataController.ChangeCoins(deltaCoins);
	}

	public void ChangeFood(int deltaFood) 
	{
		dataController.ChangeFood(deltaFood);
	}

    public void ChangeVacantHomeSlots( int deltaVacantHomeSlots)
    {
        dataController.ChangeVacantHomeSlots(deltaVacantHomeSlots);
    }
	
    public bool TryAdoptDog(DogDescriptor dog)
    {
        if (Coins.Amount < dog.CostToAdopt || VacantHomeSlots.Amount <= 0)
        {
            return false;
        }
        AdoptDog(dog);
        return true;
    }

    void AdoptDog(DogDescriptor dog)
    {
        ChangeCoins(-dog.CostToAdopt);
        ChangeVacantHomeSlots(-1);
    }

	public bool TrySendDogToScout(Dog dog) 
	{
		// Can only send a certain number of dogs out to scout
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
