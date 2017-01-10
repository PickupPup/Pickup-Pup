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
        get;
        private set;
    }

	#endregion

    CurrencySystem currencies;

	public PPGameSave(DogDescriptor[] dogs, CurrencySystem currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(dogs);
        this.Currencies = currencies;
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
