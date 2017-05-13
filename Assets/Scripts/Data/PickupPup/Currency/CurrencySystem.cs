/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann 
 * Description: Controls all forms of currency
 */

using System.Collections.Generic;
using m = MonoBehaviourExtended;
using k = PPGlobal;

[System.Serializable]
public class CurrencySystem : PPData, ICurrencySystem
{
    #region Static Accessors

    public static CurrencySystem Default
    {
        get
        {
			checkStartingValues();
            return new CurrencySystem(
				new CoinsData(startingCoins)
            );
        }
    }

    #endregion

	static int startingCoins;
	static int startingDogFood;
	static bool startingValuesInitialized;

    #region ICurrencySystem Accessors

    public CoinsData Coins
    {
        get
        {
            return currencies[CurrencyType.Coins] as CoinsData;
        }
    }

    public DogFoodData DogFood
    {
        get
        {
			return food.GetFood();
        }
    }

    #endregion


	Dictionary<CurrencyType, m.MonoActionInt> currencyChangeCallbacks
	{
		get
		{
			// Lazy reference (meaning not initialized until requested):
			if(_currencyChangeCallbacks == null)
			{
				_currencyChangeCallbacks = new Dictionary<CurrencyType, m.MonoActionInt>();
			}
			return _currencyChangeCallbacks;
		}
	}
		
	CurrencyFactory factory;
	Dictionary<CurrencyType, CurrencyData> currencies;
	FoodSystem food;

	// Includes refs to MonoBehaviours so it can not be serialized
	[System.NonSerialized]
	Dictionary<CurrencyType, m.MonoActionInt> _currencyChangeCallbacks;

    public CurrencySystem(params CurrencyData[] currencies)
    {
        this.currencies = generateCurrencyLookup(currencies);
		this.factory = new CurrencyFactory();
		this.food = new FoodSystem();
		this.food.SetAmount(startingDogFood);
    }

    #region ICurrencySystem Methods

    public void ChangeCoins(int deltaCoins)
    {
		ChangeCurrencyAmount(new CoinsData(deltaCoins));
    }

	public void ChangeFood(int deltaFood)
	{
		ChangeFood(deltaFood, k.DEFAULT_FOOD_TYPE);
	}

	public void ChangeFood(int deltaFood, string type)
    {
		food.ChangeBy(deltaFood, type);
    }

	public void ChangeCurrencyAmount(CurrencyData currency)
    {
		if(isDogFood(currency))
		{
			ChangeFood(currency.Amount, (currency as DogFoodData).FoodType);
		}
		else
		{
			CurrencyData existingCurrency = getCurrency(currency.Type);
			existingCurrency.ChangeBy(currency.Amount);
		}
		tryCallCurrencyChangeEvent(currency.Type);
    }

    public void SubscribeToCurrencyChange(CurrencyType type, m.MonoActionInt callback, bool invokeOnSubscribe)
    {
        SubscribeToCurrencyChange(type, callback);
        if(invokeOnSubscribe)
        {
            callback(currencies[type].Amount);
        }
    }

	public void SubscribeToCurrencyChange(CurrencyType type, m.MonoActionInt callback)
	{
		m.MonoActionInt handler = getCurrencyChangeEventDelegate(type);
		handler += callback;
		updateCurrencyChangeHandler(type, handler);
	}

	public void UnsubscribeFromCurrencyChange(CurrencyType type, m.MonoActionInt callback)
	{
		m.MonoActionInt handler = getCurrencyChangeEventDelegate(type);
		if(handler != null)
		{
			handler -= callback;
			updateCurrencyChangeHandler(type, handler);
		}
	}

	public void GiveCurrency(CurrencyData newCurrency)
	{
		ChangeCurrencyAmount(newCurrency);
	}
		
	public void ConvertCurrency(CurrencyData taken, CurrencyData given)
    {
		if (CanAfford(taken.Type, taken.Amount))
        {
			ChangeCurrencyAmount(taken.GetTakeAmount());
			ChangeCurrencyAmount(given);
        }
        // Otherwise do nothing
    }

	public bool TryTakeCurrency(CurrencyData currencyToTake)
	{
		CurrencyData existingCurrency = getCurrency(currencyToTake.Type);
		if(CanAfford(existingCurrency.Type, currencyToTake.Amount))
		{
			ChangeCurrencyAmount(currencyToTake.GetTakeAmount());
			return true;
		}
		else 
		{
			return false;
		}
	}

    public bool CanAfford(CurrencyType type, int cost)
    {
		if(isDogFood(type))
		{
			return food.CanAfford(cost);
		}
		else
		{
        	return currencies[type].CanAfford(cost);
		}
    }

    public bool HasCurrency(CurrencyType type)
    {
		if(isDogFood(type))
		{
			return food != null;
		}
		else
		{
        	return currencies.ContainsKey(type);
		}
    }

	#region ISubscribeable Interface // This method is also in the ICurrencySystem method (for polymorphism's sake)

	public bool TryUnsubscribeAll()
	{
		currencyChangeCallbacks.Clear();
		return true;
	}
		
    #endregion

	#endregion

	CurrencyData getCurrency(CurrencyType type)
	{
		if(isDogFood(type))
		{
			return DogFood;
		}
		else
		{
			CurrencyData currency;
			if(!currencies.TryGetValue(type, out currency))
			{
				currency = factory.Create(type.ToString(), DEFAULT_CURRENCY_AMOUNT, DEFAULT_DISCOUNT);
			}
			return currency;
		}
	}

	bool isDogFood(CurrencyData currency)
	{
		return currency is DogFoodData;
	}

	bool isDogFood(CurrencyType type)
	{
		return type == CurrencyType.DogFood;
	}

	bool tryCallCurrencyChangeEvent(CurrencyType type)
	{
		CurrencyData currency = getCurrency(type);
		return tryCallCurrencyChangeEvent(currency.Type, currency.Amount);
	}

	// Overloaded version if you want to override the currency amount:
	bool tryCallCurrencyChangeEvent(CurrencyType type, int amount)
	{
		m.MonoActionInt currencyChangeCallback = getCurrencyChangeEventDelegate(type);
		if(currencyChangeCallback != null)
		{
			currencyChangeCallback(amount);
			return true;
		}
		else 
		{
			return false;
		}
	}

	void updateCurrencyChangeHandler(CurrencyType type, m.MonoActionInt handler)
	{
		currencyChangeCallbacks[type] = handler;
	}

	m.MonoActionInt getCurrencyChangeEventDelegate(CurrencyType type)
	{
		m.MonoActionInt eventDelegate;
		if(!currencyChangeCallbacks.TryGetValue(type, out eventDelegate))
		{
			currencyChangeCallbacks.Add(type, eventDelegate);
		}
		return eventDelegate;
	}

	void addNewCurrency(CurrencyData currency)
	{
		if(isDogFood(currency))
		{	
			food.AddFood(currency as DogFoodData);
		}
		else
		{
			currencies.Add(currency.Type, currency);
		}
		addNewCurrencyToCallback(currency);
	}

	void addNewCurrencyToCallback(CurrencyData currency)
	{
		currencyChangeCallbacks.Add(currency.Type, null);
	}

	Dictionary<CurrencyType, CurrencyData> generateCurrencyLookup(CurrencyData[] currencies, bool generateCallbacks = true)
    {
        Dictionary<CurrencyType, CurrencyData> lookup = new Dictionary<CurrencyType, CurrencyData>();
        foreach (CurrencyData currency in currencies)
        {
            lookup.Add(currency.Type, currency);
			if(generateCallbacks)
			{
				addNewCurrencyToCallback(currency);
			}
        }
        return lookup;
    }

	static void checkStartingValues()
	{
		if(!startingValuesInitialized)
		{
			PPGameController game = PPGameController.GetInstance;
			bool initFromTuning = false;
			if(game)
			{
				PPTuning tuning = game.Tuning;
				if(tuning != null)
				{
					startingCoins = tuning.StartingCoins;
					startingDogFood = tuning.StartingDogFood;
					initFromTuning = true;
				}
			}
			// Fail safe method in case PPGameControlelr is not initialized yet
			if(!initFromTuning)
			{
				startingCoins = DEFAULT_COINS;
				startingDogFood = DEFAULT_DOG_FOOD;
			}
			startingValuesInitialized = true;
		}
	}

}
