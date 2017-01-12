/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann 
 * Description: Controls all forms of currency
 */

using System.Collections.Generic;

[System.Serializable]
public class CurrencySystem : PPData, ICurrencySystem
{
    #region Static Accessors

    public static CurrencySystem Default
    {
        get
        {
            return new CurrencySystem(
                new CoinsData(2000),
                new DogFoodData(0),
                new HomeSlotsData(10)
            );
        }
    }

    #endregion

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

}
