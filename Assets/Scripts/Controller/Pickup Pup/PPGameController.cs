/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
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

    static string SHOP_FILE_PATH
    {
        get
        {
            return Path.Combine(JSON_DIR, "ShopItems");
        }
    }

    static string SAVE_FILE_PATH 
	{
		get 
		{
			return Path.Combine(Application.persistentDataPath, "PickupPupSave.dat");
		}
	}

    public PPTuning Tuning
    {
        get
        {
            return tuning;
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

    public ShopDatabase Shop
    {
        get
        {
            return shop;
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

	// The dog the player currently has selected
	Dog selectedDog;
	List<Dog> dogsOutScouting = new List<Dog>();
	PPTuning tuning;
	DogDatabase database;
    ShopDatabase shop;
	PPDataController dataController;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		database = parseDatabase();
        shop = parseShopDatabase();
		tuning = parseTuning();
		database.Initialize();
        shop.Initialize();
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
		dataController.SetFilePath(SAVE_FILE_PATH);
		dataController.LoadGame();

        // Defaults not working for some reason. Help me fix!
        dataController.ChangeCoins(200);
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
	
    public bool TryBuyItem(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType)
    {
        if (Coins.Amount < cost)
        {
            return false;
        }
        BuyItem(value, valueCurrencyType, cost, costCurrencyType);
        return true;
    }

    public bool TryBuyItem(ShopItem item)
    {
        return TryBuyItem(item.Value, item.ValueCurrencyType, 
            item.Cost, item.CostCurrencyType);
    }

    void BuyItem(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType)
    {
        dataController.ChangeCurrencyByType(value, valueCurrencyType);
        dataController.ChangeCurrencyByType(-cost, costCurrencyType);
    }

    public bool TryAdoptDog(DogDescriptor dog)
    {
        if(Coins.Amount < dog.CostToAdopt || VacantHomeSlots.Amount <= 0)
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

	public void SelectDog(Dog dog)
	{
		this.selectedDog = dog;
	}
		
	public void SendSelectedDogToSlot(DogSlot slot)
	{
		sendDogToSlot(this.selectedDog, slot);
	}

	void sendDogToSlot(Dog dog, DogSlot slot)
	{
		slot.Init(dog);
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

    ShopDatabase parseShopDatabase()
    {
        TextAsset json = loadTextAssetInResources(SHOP_FILE_PATH);
        return JsonUtility.FromJson<ShopDatabase>(json.text);
    }

	PPTuning parseTuning() 
	{
		TextAsset json = loadTextAssetInResources(TUNING_FILE_PATH);
		return JsonUtility.FromJson<PPTuning>(json.text);
	}

}
