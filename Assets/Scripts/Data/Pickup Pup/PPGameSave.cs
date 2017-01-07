/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Serializable data for Pickup Pup
 */

using System.Collections.Generic;

[System.Serializable]
public class PPGameSave : GameSave 
{
	#region Instance Accessors

	public Currency Coins 
	{
		get 
		{
			return currencies[CurrencyType.Coins];
		}
	}

	public Currency Food 
	{
		get 
		{
			return currencies[CurrencyType.DogFood];
		}
	}

    public Currency OpenHomeSlots
    {
        get
        {
            return currencies[CurrencyType.OpenHomeSlots];
        }
    }

	public List<DogDescriptor> AdoptedDogs
	{
		get;
		private set;
	}

	#endregion

	Dictionary<CurrencyType, Currency> currencies;

	public PPGameSave(DogDescriptor[] dogs, params Currency[] currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(dogs);
		this.currencies = generateCurrencyLookup(currencies);
	}

	public bool HasCurrency(CurrencyType type) 
	{
		return currencies.ContainsKey(type);
	}

	public void Adopt(DogDescriptor dog) 
	{
		AdoptedDogs.Add(dog);
	}

	public void ChangeCoins(int deltaCoins) 
	{
		ChangeCurrencyAmount(CurrencyType.Coins, deltaCoins);
	}

	public void ChangeFood(int deltaFood) 
	{
		ChangeCurrencyAmount(CurrencyType.DogFood, deltaFood);
	}

    public void ChangeOpenHomeSlots(int deltaOpenHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.OpenHomeSlots, deltaOpenHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount) 
	{
		currencies[type].IncreaseBy(deltaAmount);
	}

	Dictionary<CurrencyType, Currency> generateCurrencyLookup(Currency[] currencies) 
	{
		Dictionary<CurrencyType, Currency> lookup = new Dictionary<CurrencyType, Currency>();
		foreach(Currency currency in currencies) 
		{
			lookup.Add(currency.Type, currency);
		}
		return lookup;
	}

}
