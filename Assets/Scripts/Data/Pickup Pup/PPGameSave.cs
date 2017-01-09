/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Serializable data for Pickup Pup
 */

using System.Collections.Generic;

[System.Serializable]
public class PPGameSave : GameSave 
{
	#region Instance Accessors

	public List<DogDescriptor> AdoptedDogs
	{
		get;
		private set;
	}

    public CurrencySystem Currencies
    {
        get
        {
            return currencies;
        }
    }

	#endregion

	//Dictionary<CurrencyType, CurrencyData> currencies;
    CurrencySystem currencies;

	public PPGameSave(DogDescriptor[] dogs, CurrencySystem currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(dogs);
        SaveCurrencies(currencies);
	}

    public void SaveCurrencies(CurrencySystem currencies)
    {
        this.currencies = currencies;
    }

    /*
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

    public void ChangeVacantHomeSlots(int deltaVacantHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.HomeSlots, deltaVacantHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount) 
	{
		currencies[type].IncreaseBy(deltaAmount);
	}

	Dictionary<CurrencyType, CurrencyData> generateCurrencyLookup(CurrencyData[] currencies) 
	{
		Dictionary<CurrencyType, CurrencyData> lookup = new Dictionary<CurrencyType, CurrencyData>();
		foreach(CurrencyData currency in currencies) 
		{
			lookup.Add(currency.Type, currency);
		}
		return lookup;
	}*/

}
