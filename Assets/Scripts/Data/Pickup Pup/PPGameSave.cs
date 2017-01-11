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
		
	public List<DogDescriptor> ScoutingDogs
	{
		get;
		private set;
	}

    public CurrencySystem Currencies
    {
        get;
        private set;
    }

	#endregion

    CurrencySystem currencies;

	public PPGameSave(DogDescriptor[] adoptedDogs, DogDescriptor[] scoutingDogs, CurrencySystem currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(adoptedDogs);
		this.ScoutingDogs = new List<DogDescriptor>(scoutingDogs);
        this.Currencies = currencies;
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

    public void SaveCurrencies(CurrencySystem currencies)
    {
        this.Currencies = currencies;
    }

    public void Adopt(DogDescriptor dog)
    {
        AdoptedDogs.Add(dog);
    }

}
