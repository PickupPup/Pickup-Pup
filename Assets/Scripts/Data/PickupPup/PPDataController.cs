/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles save for Pickup Pup
 */

using UnityEngine;
using System;
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

	public DateTime LastSaveTime 
	{
		get
		{
			return currentGame.TimeStamp;
		}
	}

	public float TimeInSecSinceLastSave
	{
		get
		{
			return currentGame.TimeInSecSinceLastSave;
		}
	}

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

	public float DailyGiftCountdown
	{
		get
		{
			return currentGame.DailyGiftCountdown;
		}
	}

	public bool DailyGiftCountdownRunning
	{
		get
		{
			return DailyGiftCountdown > NONE_VALUE;
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

	CurrencySystem currencies 
	{
		get 
		{
			return currentGame.Currencies;
		}
	}

	[SerializeField]
	bool saveOnApplicationPause;
	[SerializeField]
	bool saveOnApplicationQuit;

    PPGameSave currentGame;
	// For use for event subscriptions which occur before load is complete
	Dictionary<CurrencyType, Queue<MonoActionInt>> currencyChangeDelegateBuffer;
	// TODO:
	// Check for null save and add to queue
	// Dequeue into subscrption
	// Call all events after buffer is clear 

	public bool SaveGame()
	{
		Buffer(getCurrentGame());
        SaveCurrencies();
		return Save();
	}

    public PPGameSave LoadGame()
    {
        currentGame = Load() as PPGameSave;
        return currentGame;
    }

    protected PPGameSave getCurrentGame()
    {
		if(currentGame == null)
		{
			currentGame = getDefaultFile() as PPGameSave;
		}
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
	}

    #endregion

	public void SendDogToScout(Dog dog)
	{
		currentGame.SendDogToScout(dog);
		SaveGame();
	}

    public void Adopt(DogDescriptor dog)
    {
        currentGame.Adopt(dog);
        SaveGame();
    }

    public bool CheckAdopted(DogDescriptor dog)
    {
        return AdoptedDogs.Contains(dog);
    }

    #region ICurrencySystem Interface

	public void ChangeCoins(int deltaCoins) 
	{
		currencies.ChangeCoins(deltaCoins);
		SaveGame();
	}

	public void ChangeFood(int deltaFood) 
	{
        currencies.ChangeFood(deltaFood);
		SaveGame();
	}

    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        currencies.ChangeHomeSlots(deltaHomeSlots);
        SaveGame();
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
        currencies.ChangeCurrencyAmount(type, deltaAmount);
    }

	public void GiveCurrency(CurrencyData currency)
	{
		currencies.GiveCurrency(currency);
	}

    public void ConvertCurrency(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType)
    {
        currencies.ConvertCurrency(value, valueCurrencyType, cost, costCurrencyType);
    }

	public void SubscribeToCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		currencies.SubscribeToCurrencyChange(type, callback);
	}

	public void UnsubscribeFromCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		currencies.UnsubscribeFromCurrencyChange(type, callback);
	}

	public bool TryTakeCurrency(CurrencyData currency)
	{
		return currencies.TryTakeCurrency(currency);
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

	public void StartDailyGiftCountdown(PPTimer timer)
	{
		currentGame.StartDailyGiftCountdown(timer);
	}
		
}
