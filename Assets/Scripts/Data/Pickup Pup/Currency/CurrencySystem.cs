/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann 
 * Description: Controls all forms of currency
 */

using System.Collections.Generic;

[System.Serializable]
public class CurrencySystem : PPData, ICurrencySystem
{
	const int DEFAULT_COINS = 2000;
	const int DEFAULT_DOG_FOOD = 0;
	const int DEFAULT_HOME_SLOTS = 10;

    #region Static Accessors

    public static CurrencySystem Default
    {
        get
        {
			checkStartingValues();
            return new CurrencySystem(
				new CoinsData(startingCoins),
				new DogFoodData(startingDogFood),
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

    Dictionary<CurrencyType, CurrencyData> currencies;

    public CurrencySystem(params CurrencyData[] currencies)
    {
        this.currencies = generateCurrencyLookup(currencies);
    }

    #region ICurrencySystem Methods

    public void ChangeCoins(int deltaCoins)
    {
        ChangeCurrencyAmount(CurrencyType.Coins, deltaCoins);
    }

    public void ChangeFood(int deltaFood)
    {
        ChangeCurrencyAmount(CurrencyType.DogFood, deltaFood);
    }
    
    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.HomeSlots, deltaHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
        currencies[type].IncreaseBy(deltaAmount);
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

    public bool CanAfford(CurrencyType type, int cost)
    {
        return currencies[type].CanAfford(cost);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type);
    }

    #endregion

	public bool TryGetCurrency(CurrencyType type, out CurrencyData data)
	{
		return currencies.TryGetValue(type, out data);
	}

    Dictionary<CurrencyType, CurrencyData> generateCurrencyLookup(CurrencyData[] currencies)
    {
        Dictionary<CurrencyType, CurrencyData> lookup = new Dictionary<CurrencyType, CurrencyData>();
        foreach (CurrencyData currency in currencies)
        {
            lookup.Add(currency.Type, currency);
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
