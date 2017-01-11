﻿/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles save for Pickup Pup
 */

using UnityEngine;
using System.Collections.Generic;

public class PPDataController : DataController, ICurrencySystem 
{
	#region Static Accessors

	// Casts the singleton from the superclass:
	public static PPDataController GetInstance
	{
		get 
		{
			return Instance as PPDataController;
		}
	}

	#endregion

	#region Instance Accessors

	public List<DogDescriptor> AdoptedDogs 
	{
		get 
		{
			return currentGame.AdoptedDogs;
		}
	}

	public List<DogDescriptor> ScoutingDogs
	{
		get
		{
			return currentGame.ScoutingDogs;
		}
	}

	public int DogCount 
	{
		get 
		{
			return AdoptedDogs.Count;
		}
	}

    #region ICurrencySystem Accessors

    public CoinsData Coins
    {
        get
        {
            return currencies.Coins;
        }
    }

    public DogFoodData DogFood
    {
        get
        {
            return currencies.DogFood;
        }
    }

    public HomeSlotsData HomeSlots
    {
        get
        {
            return currencies.HomeSlots;
        }
    }

    #endregion

	#endregion

	[SerializeField]
	bool saveOnApplicationPause;
	[SerializeField]
	bool saveOnApplicationQuit;

    PPGameSave currentGame;
    CurrencySystem currencies;

	MonoActionInt onCoinsChange;
	MonoActionInt onFoodChange;
    MonoActionInt onHomeSlotsChange;

	public bool SaveGame()
	{
		Buffer(getCurrentGame());
        SaveCurrencies();
		return Save();
	}

    public PPGameSave LoadGame()
    {
        currentGame = Load() as PPGameSave;
        currencies = currentGame.Currencies;
        return currentGame;
    }

    protected PPGameSave getCurrentGame()
    {
        return currentGame;
    }

    public void SaveCurrencies()
    {
        currentGame.SaveCurrencies(currencies);
    }
		
	#region MonoBehaviourExtended Overrides

	protected override void handleGameTogglePause(bool isPaused)
	{
		// Avoids saving twice in the same call on iOS: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationPause.html
		#if UNITY_IOS
		if(saveOnApplicationPause && !saveOnApplicationQuit)
		{
			SaveGame();
		}
		#else
		if(saveOnApplicationPause)
		{
			SaveGame();
		}
		#endif
	}

	protected override void handleGameQuit()
	{
		if(saveOnApplicationQuit)
		{
			SaveGame();
		}
	}
		
	#endregion

    #region DataController Overrides

    protected override SerializableData getDefaultFile() 
	{
		return new PPGameSave(new DogDescriptor[0], new DogDescriptor[0], CurrencySystem.Default);
	}		
		
	public override void Reset() 
	{
		base.Reset();
		LoadGame();
		callOnCoinsChange(Coins.Amount);
		callOnFoodChange(DogFood.Amount);
        callOnHomeSlotsChange(HomeSlots.Amount);
	}

    #endregion

    #region Event Subscription

    public void SubscribeToCoinsChange(MonoActionInt coinsAction)
    {
        onCoinsChange += coinsAction;
    }

    public void UnsubscribeFromCoinsChange(MonoActionInt coinsAction)
    {
        onCoinsChange -= coinsAction;
    }

    public void SubscribeToFoodChange(MonoActionInt foodAction)
    {
        onFoodChange += foodAction;
    }

    public void UnsubscribeToFoodChange(MonoActionInt foodAction)
    {
        onFoodChange -= foodAction;
    }

    public void SubscribeToHomeSlotsChange(MonoActionInt HomeSlotsAction)
    {
        onHomeSlotsChange += HomeSlotsAction;
    }

    public void UnsubscribeToHomeSlotsChange(MonoActionInt HomeSlotsAction)
    {
        onHomeSlotsChange -= HomeSlotsAction;
    }

    #endregion

    #region Event Calls

    protected void callOnCoinsChange(int coins) 
	{ 
		if(onCoinsChange != null)
		{
			onCoinsChange(coins);
		}
	}

	protected void callOnFoodChange(int food)
	{
		if(onFoodChange != null) 
		{
			onFoodChange(food);
		}
	}

    protected void callOnHomeSlotsChange(int homeSlots)
    {
        if(onHomeSlotsChange != null)
        {
            onHomeSlotsChange(homeSlots);
        }
    }

    #endregion

	public void SendDogToScout(Dog dog)
	{
		currentGame.SendDogToScout(dog);
		UnityEngine.Debug.Log("SAVING");
		SaveGame();
	}

    public void Adopt(DogDescriptor dog)
    {
        currentGame.Adopt(dog);
        SaveGame();
    }

    #region ICurrencySystem Interface

	public void ChangeCoins(int deltaCoins) 
	{
		currencies.ChangeCoins(deltaCoins);
		callOnCoinsChange(Coins.Amount);
		SaveGame();
	}

	public void ChangeFood(int deltaFood) 
	{
        currencies.ChangeFood(deltaFood);
		callOnFoodChange(DogFood.Amount);
		SaveGame();
	}

    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        currencies.ChangeHomeSlots(deltaHomeSlots);
        callOnHomeSlotsChange(HomeSlots.Amount);
        SaveGame();
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
        currencies.ChangeCurrencyAmount(type, deltaAmount);
    }

    public void ConvertCurrency(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType)
    {
        currencies.ConvertCurrency(value, valueCurrencyType, cost, costCurrencyType);
    }

    public bool CanAfford(CurrencyType type, int amount)
    {
        return currencies.CanAfford(type, amount);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return HasCurrency(type);
    }

    #endregion

}
