﻿/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Game controller for Pickup Pup
 */

using System.IO;
using UnityEngine;
using System.Collections.Generic;
using k = PPGlobal;

public class PPGameController : GameController, ICurrencySystem 
{
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
			return Path.Combine(k.JSON_DIR, k.TUNING);
		}
	}

	static string GAME_DATA_FILE_PATH 
	{
		get 
		{
			return Path.Combine(k.JSON_DIR, k.GAME_DATA);
		}
	}

    static string SHOP_FILE_PATH
    {
        get
        {
			return Path.Combine(k.JSON_DIR, k.SHOP_ITEMS);
        }
    }

	static string GIFT_FILE_PATH
    {
        get
        {
			return Path.Combine(k.JSON_DIR, k.GIFT_ITEMS);
        }
    }

    static string SAVE_FILE_PATH 
	{
		get 
		{
			return Path.Combine(Application.persistentDataPath, k.SAVE_FILE);
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

	public DogDatabase DogData
	{
		get 
		{
			return dogDatabase; 
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
		
	public GiftDatabase Gifts
    {
        get
        {
            return gifts;
        }
    }
		
    #region ICurrencySystem Interface

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

	public bool HasTargetSlot
	{
		get
		{
			return targetSlot != null;
		}
	}

	#endregion

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
	DogDatabase dogDatabase;
    ShopDatabase shop;
	GiftDatabase gifts;
	LanguageDatabase languages;
	PPDataController dataController;
	PPGiftController giftController;
	DogSlot targetSlot;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		dogDatabase = parseDogDatabase();
        shop = parseShopDatabase();
		gifts = parseGiftDatabase();
		tuning = parseTuning();
		languages = LanguageDatabase.Instance;
		languages.Initialize();
		dogDatabase.Initialize();
        shop.Initialize();
		gifts.Initialize();
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
		dataController.SetFilePath(SAVE_FILE_PATH);
		dataController.LoadGame();
		giftController = PPGiftController.Instance;
		giftController.Init(tuning);
		handleLoadGame(dataController);
	}

	void handleLoadGame(PPDataController dataController)
	{
		List<DogDescriptor> dogs = dataController.ScoutingDogs;
		if(dogs != null && dogs.Count > 0)
		{
			Dog[] dogObjs = new DogFactory(hideGameObjects:true).CreateGroup(dogs.ToArray());
			dogsOutScouting = new List<Dog>(dogObjs);
			callScoutingDogsLoaded(dogObjs);
		}
	}

	void callScoutingDogsLoaded(Dog[] dogs)
	{
		foreach(Dog dog in dogs)
		{
			dog.SetGame(this);
			dog.SetTimer(dog.Info.TimeRemainingScouting);
			dog.Info.HandleScoutingBegan(dog.Info.ScoutingSlotIndex);
			EventController.Event(PPEvent.ScoutingDogLoaded, dog);
		}
	}

	public int GetCurrentSlotIndex()
	{
		if(HasTargetSlot)
		{
			return targetSlot.GetIndex();	
		}
		else
		{
			return INVALID_VALUE;
		}
	}

    #endregion

    #region ICurrencySystem Interface

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

	public void SetTargetSlot(DogSlot slot)
	{
		this.targetSlot = slot;
	}
		
	public void GiveCurrency(CurrencyData currency)
	{
		dataController.GiveCurrency(currency);
	}

	public void SubscribeToCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		dataController.SubscribeToCurrencyChange(type, callback);
	}

	public void UnsubscribeFromCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		dataController.UnsubscribeFromCurrencyChange(type, callback);
	}

	bool ICurrencySystem.TryUnsubscribeAll()
	{
		(dataController as ICurrencySystem).TryUnsubscribeAll();
		return true;
	}

    public bool CanAfford(CurrencyType type, int amount)
    {
        return dataController.CanAfford(type, amount);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return dataController.HasCurrency(type);
    }

	public bool TryTakeCurrency(CurrencyData currency)
	{
		return dataController.TryTakeCurrency(currency);
	}

    #endregion

	public CurrencyData GetGift(DogDescriptor dog)
	{
		CurrencyData data = giftController.GetGiftFromDog(dog);
		dataController.ChangeCurrencyAmount(data.Type, data.Amount);
		return data;
	}

    public bool TryBuyItem(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType)
    {
        if (CanAfford(costCurrencyType, cost))
        {
			print("Failed To Buy");
            return false;
        }
        buyItem(value, valueCurrencyType, cost, costCurrencyType);
        return true;
    }

    public bool TryBuyItem(ShopItem item)
    {
		return TryBuyItem(item.Value, item.ValueCurrencyType, item.Cost, item.CostCurrencyType);
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
        dataController.Adopt(dog);
    }

	public bool TrySendDogToScout(Dog dog, out int slotIndex)
	{
		// Can only send a certain number of dogs out to scout
		if(DogsScoutingAtCapacity || dogsOutScouting.Contains(dog)) 
		{
			slotIndex = INVALID_VALUE;
			return false;
		} 
		else 
		{
			slotIndex = targetSlot.transform.GetSiblingIndex();
			sendDogToScout(dog);
			dataController.SendDogToScout(dog);
			return true;
		}
	}

	public void SelectDog(Dog dog)
	{
		this.selectedDog = dog;
	}

	public void SendToTargetSlot(Dog dog)
	{
		if(HasTargetSlot)
		{
			targetSlot.Init(dog);
			ClearTargetSlot();
		}
	}

	public void ClearTargetSlot()
	{
		this.targetSlot = null;
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
        EventController.Event("PlayDogSendOut");
		dogsOutScouting.Add(dog);
		dog.SubscribeToScoutingTimerEnd(handleDogDoneScouting);
	}

	void handleDogDoneScouting(Dog dog) 
	{
        EventController.Event("PlayDogReturn");
        dogsOutScouting.Remove(dog);
		// Need to unsubscribe to prevent stacking even subscriptions if dog is sent to scout again:
		dog.UnsubscribeFromScoutingTimerEnd(handleDogDoneScouting);
	}

    public bool TryRedeemGift(int value, CurrencyType valueCurrencyType)
    {
        RedeemGift(value, valueCurrencyType);
        return true;
    }

	public bool TryRedeemGift(GiftItem gift)
    {
        return TryRedeemGift(gift.Value, gift.ValueType);
    }

	public void RedeemGift(int value, CurrencyType valueCurrencyType)
    {
		dataController.ChangeCurrencyAmount(valueCurrencyType, value);
    }

	DogDatabase parseDogDatabase() 
	{
        return parseFromJSONInResources<DogDatabase>(GAME_DATA_FILE_PATH);
	}

    ShopDatabase parseShopDatabase()
    {
        return parseFromJSONInResources<ShopDatabase>(SHOP_FILE_PATH);
    }

	GiftDatabase parseGiftDatabase()
    {
        TextAsset json = loadTextAssetInResources(GIFT_FILE_PATH);
        return JsonUtility.FromJson<GiftDatabase>(json.text);
    }

	PPTuning parseTuning() 
	{
        return parseFromJSONInResources<PPTuning>(TUNING_FILE_PATH);
	}

}