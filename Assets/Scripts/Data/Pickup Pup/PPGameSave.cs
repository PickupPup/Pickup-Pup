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

    public Currency VacantHomeSlots
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

	public List<DogDescriptor> ScoutingDogs
	{
		get;
		private set;
	}

	#endregion

	Dictionary<CurrencyType, Currency> currencies;

	public PPGameSave(DogDescriptor[] dogs, DogDescriptor[] scoutingDogs, params Currency[] currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(dogs);
		this.ScoutingDogs = new List<DogDescriptor>(scoutingDogs);
		this.currencies = generateCurrencyLookup(currencies);
	}
		
	public void SendDogToScout(Dog dog)
	{
		if(!ScoutingDogs.Contains(dog.Info)) 
		{
			ScoutingDogs.Add(dog.Info);
			dog.SubscribeToScoutingTimerEnd(handleDogFinishdScouting);
		}
	}

	void handleDogFinishdScouting(Dog dog)
	{
		ScoutingDogs.Remove(dog.Info);
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
