/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann, Ben Page
 * Description: Controls all forms of currency
 */
using UnityEngine;
using System.Collections.Generic;
using m = MonoBehaviourExtended;

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
				new CoinsData(startingCoins),
				new DogFoodData(),
                new HomeSlotsData(startingHomeSlots)
            );
        }
    }

    #endregion

	static int startingCoins;
	static int startingDogFood;
    static int startingHomeSlots;
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
            return currencies[CurrencyType.DogFood] as DogFoodData;
        }
    }

    public HomeSlotsData HomeSlots
    {
        get
        {
            return currencies[CurrencyType.HomeSlots] as HomeSlotsData;
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

    Dictionary<DogFoodType, m.MonoActionInt> dogFoodChangeCallbacks
    {
        get
        {
            // Lazy reference (meaning not initialized until requested):
            if (_dogFoodChangeCallbacks == null)
            {
                _dogFoodChangeCallbacks = new Dictionary<DogFoodType, m.MonoActionInt>();
            }
            return _dogFoodChangeCallbacks;
        }
    }

    CurrencyFactory factory;
	Dictionary<CurrencyType, CurrencyData> currencies;

	// Includes refs to MonoBehaviours so it can not be serialized
	[System.NonSerialized]
	Dictionary<CurrencyType, m.MonoActionInt> _currencyChangeCallbacks;
    [System.NonSerialized]
    Dictionary<DogFoodType, m.MonoActionInt> _dogFoodChangeCallbacks;

    public CurrencySystem(params CurrencyData[] currencies)
    {
        this.currencies = generateCurrencyLookup(currencies);
		factory = new CurrencyFactory();
    }

    #region ICurrencySystem Methods

    public void ChangeCoins(int deltaCoins)
    {
        ChangeCurrencyAmount(CurrencyType.Coins, deltaCoins);
    }

    public void ChangeFood(int deltaFood, DogFoodType foodType)
    {
        ChangeDogFoodAmount(CurrencyType.DogFood, deltaFood, foodType);
    }

    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.HomeSlots, deltaHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
		CurrencyData existingCurrency = getCurrency(type);
		existingCurrency.ChangeBy(deltaAmount);
		tryCallCurrencyChangeEvent(type);
    }
    
    // Accounts for dog food types
    public void ChangeDogFoodAmount(CurrencyType type, int deltaAmount, DogFoodType dogFoodType)
    {
        Debug.Log(type + " " + dogFoodType);
        CurrencyData existingCurrency = getCurrency(type);
        existingCurrency.ChangeBy(deltaAmount, dogFoodType);
        //tryCallCurrencyChangeEvent(type);
        //UPDATE UI
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
        //Debug.Log(handler.Method);
        //Debug.Log(handler.Target);
        //Debug.Log(callback.Method);
        //Debug.Log(callback.Target);
        updateCurrencyChangeHandler(type, handler);
	}

    public void SubscribeToDogFoodChange(CurrencyType type, m.MonoActionInt callback, DogFoodType dogFoodType)
    {
        m.MonoActionInt handler = getDogFoodChangeEventDelegate(type, dogFoodType);
        handler += callback;
        //Debug.Log(handler.Method);
        // Debug.Log(handler.Target);
        //Debug.Log(callback.Method);
        // Debug.Log(callback.Target);
        updateDogFoodChangeHandler(type, handler, dogFoodType);
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
		ChangeCurrencyAmount(newCurrency.Type, newCurrency.Amount);
	}
		
    public void ConvertCurrency(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType)
    {
        if (CanAfford(costCurrencyType, cost))
        {
            ChangeCurrencyAmount(valueCurrencyType, value);
            ChangeCurrencyAmount(costCurrencyType, -cost);
        }
        // Otherwise do nothing
    }

    public void ConvertDogFood(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType, DogFoodType dogFoodType)
    {
        //Debug.Log("8");
        if (CanAfford(costCurrencyType, cost))
        {
            //Debug.Log("9");
            ChangeDogFoodAmount(valueCurrencyType, value, dogFoodType);
            ChangeCurrencyAmount(costCurrencyType, -cost);
        }
        // Otherwise do nothing
    }

    public bool TryTakeCurrency(CurrencyData currencyToTake)
	{
		CurrencyData existingCurrency;
		if(TryGetCurrency(currencyToTake.Type, out existingCurrency))
		{
			if(CanAfford(existingCurrency.Type, currencyToTake.Amount))
			{
				ChangeCurrencyAmount(existingCurrency.Type, currencyToTake.Amount);
				return true;
			}
			else 
			{
				return false;
			}
		}
		else 
		{
			return false;
		}
	}

    public bool CanAfford(CurrencyType type, int cost)
    {
        return currencies[type].CanAfford(cost);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type);
    }

	#region ISubscribeable Interface // This method is also in the ICurrencySystem method (for polymorphism's sake)

	public bool TryUnsubscribeAll()
	{
        currencyChangeCallbacks.Clear();
        dogFoodChangeCallbacks.Clear();
		return true;
	}
		
    #endregion

	#endregion

	CurrencyData getCurrency(CurrencyType type)
	{
		CurrencyData currency;
		if(!currencies.TryGetValue(type, out currency))
		{
			currency = factory.Create(type.ToString(), DEFAULT_CURRENCY_AMOUNT, DEFAULT_DISCOUNT);
		}
		return currency;
	}

    FoodItem getFoodItem(DogFoodType dogFoodType)
    {
        return FoodDatabase.Instance.Food[(int)dogFoodType];
    }

    bool tryCallCurrencyChangeEvent(CurrencyType type)
	{
		CurrencyData currency = getCurrency(type);
		return tryCallCurrencyChangeEvent(currency.Type, currency.Amount);
	}

    bool tryCallDogFoodChangeEvent(DogFoodType dogFoodType)
    {
        FoodItem foodItem = getFoodItem(dogFoodType);
        return tryCallDogFoodChangeEvent(foodItem.FoodType, foodItem.CurrentAmount);
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

    // Overloaded version if you want to override the currency amount:
    bool tryCallDogFoodChangeEvent(DogFoodType Foodtype, int amount)
    {
        m.MonoActionInt dogFoodChangeCallback = getDogFoodChangeEventDelegate(CurrencyType.DogFood, Foodtype);
        if (dogFoodChangeCallback != null)
        {
            dogFoodChangeCallback(amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    void updateCurrencyChangeHandler(CurrencyType type, m.MonoActionInt handler)
	{
		if(currencyChangeCallbacks.ContainsKey(type))
		{
			currencyChangeCallbacks[type] = handler;
		}
		else 
		{
			currencyChangeCallbacks.Add(type, handler);
		}
	}

    void updateDogFoodChangeHandler(CurrencyType type, m.MonoActionInt handler, DogFoodType dogFoodType)
    {
        if (dogFoodChangeCallbacks.ContainsKey(dogFoodType))
        {
            dogFoodChangeCallbacks[dogFoodType] = handler;
        }
        else
        {
            dogFoodChangeCallbacks.Add(dogFoodType, handler);
        }
    }

    public bool TryGetCurrency(CurrencyType type, out CurrencyData data)
	{
		return currencies.TryGetValue(type, out data);
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

    m.MonoActionInt getDogFoodChangeEventDelegate(CurrencyType type, DogFoodType dogFoodType)
    {
        m.MonoActionInt eventDelegate;
        if (!dogFoodChangeCallbacks.TryGetValue(dogFoodType, out eventDelegate))
        {
            dogFoodChangeCallbacks.Add(dogFoodType, eventDelegate);
        }
        return eventDelegate;
    }

    void addNewCurrency(CurrencyData currency)
	{
		currencies.Add(currency.Type, currency);
		addNewCurrencyToCallback(currency);
	}

	void addNewCurrencyToCallback(CurrencyData currency)
	{
		currencyChangeCallbacks.Add(currency.Type, null);
	}

    void addNewDogFoodToCallback(DogFoodType foodType)
    {
        dogFoodChangeCallbacks.Add(foodType, null);
    }

    Dictionary<CurrencyType, CurrencyData> generateCurrencyLookup(CurrencyData[] currencies, bool generateCallbacks = true)
    {
        Debug.Log(currencies[0]);
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
					startingHomeSlots = tuning.StartingHomeSlots;
					initFromTuning = true;
				}
			}
			// Fail safe method in case PPGameControlelr is not initialized yet
			if(!initFromTuning)
			{
				startingCoins = DEFAULT_COINS;
				startingDogFood = DEFAULT_DOG_FOOD;
				startingHomeSlots = DEFAULT_HOME_SLOTS;
			}
			startingValuesInitialized = true;
		}
	}

}
