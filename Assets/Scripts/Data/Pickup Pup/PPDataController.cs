/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles save for Pickup Pup
 */

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

	public List<DogDescriptor> AdoptedDogs 
	{
		get 
		{
			return currentGame.AdoptedDogs;
		}
	}

	public int DogCount 
	{
		get 
		{
			return AdoptedDogs.Count;
		}
	}

    #endregion

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

    PPGameSave currentGame;
    CurrencySystem currencies;
	MonoActionInt onCoinsChange;
	MonoActionInt onFoodChange;
    MonoActionInt onHomeSlotsChange;

	public bool SaveGame()
	{
		Buffer(getCurrentGame());
		return Save();
	}

    public void SaveCurrencies(CurrencySystem currencies)
    {
        currentGame.SaveCurrencies(currencies);
        SaveGame();
    }

    public PPGameSave LoadGame()
    {
        currentGame = Load() as PPGameSave;
        currencies = currentGame.Currencies;
        UnityEngine.Debug.Log(currencies == null);
        UnityEngine.Debug.Log(currencies.Coins == null);
        if (currencies.Coins.Amount == 0)
        {
            ChangeCoins(2000); // Used for Debugging only
        }
        return currentGame;
    }

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
		
	#region DataController Overrides

	protected override SerializableData getDefaultFile() 
	{
        UnityEngine.Debug.Log("getting default file");
		return new PPGameSave(new DogDescriptor[0], CurrencySystem.Default);
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

    protected PPGameSave getCurrentGame() 
	{
		return currentGame;
	}

    public void Adopt(DogDescriptor dog)
    {
        Adopt(dog);
        SaveGame();
    }

    #region ICurrencySystem Methods

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
        throw new NotImplementedException();
    }

    public bool HasCurrency(CurrencyType type)
    {
        return HasCurrency(type);
    }

    #endregion

}
