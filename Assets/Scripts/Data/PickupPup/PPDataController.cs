/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles save for Pickup Pup
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using k = PPGlobal;

public class PPDataController : DataController, ICurrencySystem 
{
    const bool FREE_STARTING_DOGS_ENABLED = false;

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

    public DogDescriptor[] AvailableDogs
    {
        get
        {
            return currentGame.AvailableDogs;
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

    public bool HasGiftToRedeem
    {
        get
        {
            return currentGame.HasGiftToRedeem;
        }
    }

    public bool ShouldGiveFreeDogs 
    {
        get
        {
            return FREE_STARTING_DOGS_ENABLED && DogCount == NONE_VALUE;
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

    bool hasCurrencySystem
    {
        get
        {
            return currencies != null;
        }
    }

	[SerializeField]
	bool saveOnApplicationPause;
	[SerializeField]
	bool saveOnApplicationQuit;

    PPGameSave currentGame;
	
    // For use for event subscriptions which occur before load is complete
    Dictionary<CurrencyType, Queue<MonoActionInt>> currencyChangeDelegateBuffers = new Dictionary<CurrencyType, Queue<MonoActionInt>>();

	public bool SaveGame()
	{
		Buffer(getCurrentGame());
        SaveCurrencies();
		return Save();
	}

    public PPGameSave LoadGame()
    {
        currentGame = Load() as PPGameSave;
        drainBufferedCurrencyDelegatesIntoCurrencySystem(currentGame.Currencies);
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
	
    public bool AllDogsAdopted(DogDatabase dogDatabase)
    {
        return dogDatabase.Dogs.Length <= AdoptedDogs.Count;
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
        // Destroy the singleton so it's a true reset
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
        EventController.Event(k.ADOPT, dog.PeekDogLink);
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
        if(hasCurrencySystem)
        {
		    currencies.SubscribeToCurrencyChange(type, callback);
        }
        else
        {
            bufferChangeCurrencyDelegate(type, callback);
        }
	}

	public void UnsubscribeFromCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
        if(hasCurrencySystem)
        {
		    currencies.UnsubscribeFromCurrencyChange(type, callback);
        }
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
	
    public void RedeemGift(CurrencyData gift)
    {
        currentGame.RedeemGift(gift);
    }

    public void NotifyHasGiftToRedeem()
    {
        currentGame.NotifyHasGiftToRedeem();
    }

    void bufferChangeCurrencyDelegate(CurrencyType type, MonoActionInt callback)
    {
        Queue<MonoActionInt> bufferedCallbacks;
        if(!currencyChangeDelegateBuffers.TryGetValue(type, out bufferedCallbacks))
        {
            bufferedCallbacks = new Queue<MonoActionInt>();
        }
        bufferedCallbacks.Enqueue(callback);
    }

    void drainBufferedCurrencyDelegatesIntoCurrencySystem(CurrencySystem system)
    {
        foreach(CurrencyType currency in currencyChangeDelegateBuffers.Keys)
        {
            Queue<MonoActionInt> buffer = currencyChangeDelegateBuffers[currency];
            while(buffer.Count > 0)
            {
                // Invokes upon subscription to ensure any delegates expecting initial calls are satisfied
                system.SubscribeToCurrencyChange(currency, buffer.Dequeue(), invokeOnSubscribe:true);
            }
        }
        currencyChangeDelegateBuffers.Clear();
    }

}
