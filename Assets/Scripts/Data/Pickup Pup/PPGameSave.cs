/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Serializable data for Pickup Pup
 */

using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class PPGameSave : GameSave, ISerializable
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

	public PPGameSave(DogDescriptor[] adoptedDogs, DogDescriptor[] scoutingDogs, CurrencySystem currencies) 
	{
		this.AdoptedDogs = new List<DogDescriptor>(adoptedDogs);
		this.ScoutingDogs = new List<DogDescriptor>(scoutingDogs);
        this.Currencies = currencies;
	}

	#region ISerializable Interface 

	// The special constructor is used to deserialize values.
	public PPGameSave(SerializationInfo info, StreamingContext context) : 
	base(info, context)
	{
		this.AdoptedDogs = info.GetValue(ADOPTED, typeof(List<DogDescriptor>)) as List<DogDescriptor>;
		this.ScoutingDogs = info.GetValue(SCOUTING, typeof(List<DogDescriptor>)) as List<DogDescriptor>;
		foreach(DogDescriptor dog in this.ScoutingDogs)
		{
			dog.UpdateFromSave(this);
		}
		this.Currencies = info.GetValue(CURRENCY, typeof(CurrencySystem)) as CurrencySystem;
	}
		
	// Implement this method to serialize data. The method is called on serialization.
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue(ADOPTED, this.AdoptedDogs);
		info.AddValue(SCOUTING, this.ScoutingDogs);
		info.AddValue(CURRENCY, this.Currencies);
	}

	#endregion

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
