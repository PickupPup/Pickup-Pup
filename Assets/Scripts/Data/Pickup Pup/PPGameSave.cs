/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Serializable data for Pickup Pup
 */

using System.Collections.Generic;

[System.Serializable]
public class PPGameSave : GameSave 
{
	#region Instance Accessors

	public CurrencyData Coins 
	{
		get 
		{
			return currencies[CurrencyType.Coins];
		}
	}

	public CurrencyData Food 
	{
		get 
		{
			return currencies[CurrencyType.DogFood];
		}
	}

    public CurrencyData VacantHomeSlots
    {
        get
        {
            return currencies[CurrencyType.VacantHomeSlots];
        }
    }

	public List<DogDescriptor> AdoptedDogs
	{
		get;
		private set;
	}

	#endregion

	Dictionary<CurrencyType, CurrencyData> currencies;

	public PPGameSave(DogDescriptor[] dogs, params CurrencyData[] currencies) 
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

    public void ChangeVacantHomeSlots(int deltaVacantHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.VacantHomeSlots, deltaVacantHomeSlots);
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
	}

}
