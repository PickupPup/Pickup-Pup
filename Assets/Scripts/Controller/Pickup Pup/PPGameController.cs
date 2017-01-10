/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Game controller for Pickup Pup
 */

using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public class PPGameController : GameController, ICurrencySystem 
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

	#region Instance Accessors

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

    public bool DogsScoutingAtCapacity
    {
        get
        {
            return dogsOutScouting.Count >= tuning.MaxDogsScouting;
        }
    }

    #endregion

    #region ICurrencySystem Accessors

    public CoinsData Coins
    {
        get
        {
            return dataController.Coins;
        }
    }

    public DogFoodData DogFood
    {
        get
        {
            return dataController.DogFood;
        }
    }

    public HomeSlotsData HomeSlots
    {
        get
        {
            return dataController.HomeSlots;
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
	}

    #endregion

    #region ICurrencySystem Methods

    public void ChangeCoins(int deltaCoins) 
	{
		dataController.ChangeCoins(deltaCoins);
	}

	public void ChangeFood(int deltaFood) 
	{
		dataController.ChangeFood(deltaFood);
	}

    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        dataController.ChangeHomeSlots(deltaHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
        dataController.ChangeCurrencyAmount(type, deltaAmount);
    }

    public void ConvertCurrency(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType)
    {
        dataController.ConvertCurrency(value, valueCurrencyType, cost, costCurrencyType);
    }

    public bool CanAfford(CurrencyType type, int amount)
    {
        return dataController.CanAfford(type, amount);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return dataController.HasCurrency(type);
    }

    #endregion

    public bool TryBuyItem(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType)
    {
        if (CanAfford(costCurrencyType, cost))
        {
            return false;
        }
        buyItem(value, valueCurrencyType, cost, costCurrencyType);
        return true;
    }

    public bool TryBuyItem(ShopItem item)
    {
        return TryBuyItem(item.Value, item.ValueCurrencyType, 
            item.Cost, item.CostCurrencyType);
    }

    void buyItem(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType)
    {
        ConvertCurrency(value, valueCurrencyType, cost, costCurrencyType);
    }

    public bool TryAdoptDog(DogDescriptor dog)
    {
        if(CanAfford(CurrencyType.Coins, dog.CostToAdopt) && CanAfford(CurrencyType.HomeSlots, 1))
        {
            AdoptDog(dog);
            return true;
        }        
        return false;       
    }

    void AdoptDog(DogDescriptor dog)
    {
        dataController.ChangeCoins(-dog.CostToAdopt);
        dataController.ChangeHomeSlots(-1);
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
