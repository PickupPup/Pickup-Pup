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

    // Dogs that are neither already in the world or scouting
    public DogDescriptor[] AvailableDogs
    {
        get
        {
            return getAvailableDogs();
        }
    }

	public DogDescriptor[] HomeDogs
	{
		get
		{
			return getDogsHome();
		}
	}

    public Dictionary<PPScene, List<DogDescriptor>> WorldDogs
    {
        get;
        private set;
    }

    public CurrencySystem Currencies
    {
        get;
        private set;
    }
		
	// In seconds:
	public float DailyGiftCountdown
	{
		get;
		private set;
	}

	// The player gets their first gift w/out having to wait 24 hours
	public bool HasGiftToRedeem
	{
		get;
		private set;
	}

    public TimeSpan TimePlayed
	{
        get
        {
            return this.cumulativeTimePlayed + getTimePlayedInSession();
        }
	}

	public int GameSessionCount
	{
		get;
		private set;
	}
		
	#endregion

    TimeSpan cumulativeTimePlayed = TimeSpan.Zero;
    DateTime lastTimeLoaded = default(DateTime);

	public PPGameSave(DogDescriptor[] adoptedDogs, DogDescriptor[] scoutingDogs, CurrencySystem currencies, bool hasGiftToRedeem = true)
	{
		this.AdoptedDogs = new List<DogDescriptor>(adoptedDogs);
		this.ScoutingDogs = new List<DogDescriptor>(scoutingDogs);
        this.WorldDogs = new Dictionary<PPScene, List<DogDescriptor>>();
        this.Currencies = currencies;
        this.HasGiftToRedeem = hasGiftToRedeem;
        this.lastTimeLoaded = DateTime.Now;
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
        this.WorldDogs = info.GetValue(WORLD, typeof(Dictionary<PPScene, List<DogDescriptor>>)) as Dictionary<PPScene, List<DogDescriptor>>;
		this.Currencies = info.GetValue(CURRENCY, typeof(CurrencySystem)) as CurrencySystem;
		this.DailyGiftCountdown = (float) info.GetValue(DAILY_GIFT_COUNTDOWN, typeof(float));
		this.DailyGiftCountdown -= TimeInSecSinceLastSave;
		if(this.DailyGiftCountdown < 0)
		{
			this.DailyGiftCountdown = 0;
		}
		this.HasGiftToRedeem = (bool) info.GetValue(HAS_GIFT_TO_REDEEM, typeof(bool));
        this.GameSessionCount = ((int) info.GetValue(SESSION_COUNT, typeof(int))) + SINGLE_VALUE;
        this.cumulativeTimePlayed = (TimeSpan) info.GetValue(TIME_PLAYED, typeof(TimeSpan));
        this.lastTimeLoaded = DateTime.Now;
	}
		
	// Implement this method to serialize data. The method is called on serialization.
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue(ADOPTED, this.AdoptedDogs);
		info.AddValue(SCOUTING, this.ScoutingDogs);
		info.AddValue(CURRENCY, this.Currencies);
		info.AddValue(DAILY_GIFT_COUNTDOWN, this.DailyGiftCountdown);
		info.AddValue(HAS_GIFT_TO_REDEEM, this.HasGiftToRedeem);
        info.AddValue(WORLD, this.WorldDogs);
        info.AddValue(SESSION_COUNT, this.GameSessionCount);
        cumulativeTimePlayed += getTimePlayedInSession();
        info.AddValue(TIME_PLAYED, this.cumulativeTimePlayed);
		updateScoutingDogs();
	}

	#endregion

	public DogFoodData[] GetAvailableFoods()
	{
		return Currencies.GetAvailableFoods();
	}

    public void RedeemGift(CurrencyData gift)
    {
        HasGiftToRedeem = false;
        Currencies.GiveCurrency(gift);
    }

    public void NotifyHasGiftToRedeem()
    {
        HasGiftToRedeem = true;
    }

	public void SendDogToScout(Dog dog)
	{
		if(!ScoutingDogs.Contains(dog.Info)) 
		{
			ScoutingDogs.Add(dog.Info);
		}
	}

    public void ClearScoutingDogs()
    {
        ScoutingDogs.Clear();
    }

	public void StartDailyGiftCountdown(PPTimer timer)
	{
		timer.SubscribeToTimeChange(updateDailyGiftCountdown);
	}

	void updateDailyGiftCountdown(float timeRemaining)
	{
		this.DailyGiftCountdown = timeRemaining;
	}
                    
    public void SaveCurrencies(CurrencySystem currencies)
    {
        this.Currencies = currencies;
    }

    public void Adopt(DogDescriptor dog)
    {
        AdoptedDogs.Add(dog);
    }
	
    public void EnterRoom(DogDescriptor dog, PPScene room)
    {
        dog.EnterRoom(room);
        addDogToRoom(dog, room);
    }

    public void LeaveRoom(DogDescriptor dog)
    {
        removeFromRoom(dog, dog.MostRecentRoom);
        dog.LeaveRoom();
    }

    public DogDescriptor[] DogsInRoom(PPScene room)
    {
        List<DogDescriptor> inRoom;
        if(WorldDogs.TryGetValue(room, out inRoom))
        {
            return inRoom.ToArray();
        }
        else
        {
            return new DogDescriptor[NONE_VALUE];
        }
    }

    void addDogToRoom(DogDescriptor dog, PPScene room)
    {
        List<DogDescriptor> inRoom;
        if(!WorldDogs.TryGetValue(room, out inRoom))
        {
            inRoom = new List<DogDescriptor>();
            WorldDogs.Add(room, inRoom);
        }
        inRoom.Add(dog);
    }

    void removeFromRoom(DogDescriptor dog, PPScene room)
    {
        List<DogDescriptor> inRoom;
        if(WorldDogs.TryGetValue(room, out inRoom))
        {
            inRoom.Remove(dog);
        }
    }

	void updateScoutingDogs()
	{
		foreach(DogDescriptor dog in AdoptedDogs)
		{
			if(dog.IsScouting && !ScoutingDogs.Contains(dog))
			{
				dog.HandleScoutingEnded();
			}
		}
	}

	DogDescriptor[] getDogsHome()
	{
		List<DogDescriptor> home = new List<DogDescriptor>();
		foreach(DogDescriptor dog in AdoptedDogs)
		{
			if(!dog.IsScouting)
			{
				home.Add(dog);
			}
		}
		return home.ToArray();
	}

    DogDescriptor[] getAvailableDogs()
    {
        List<DogDescriptor> available = new List<DogDescriptor>();
        foreach(DogDescriptor dog in AdoptedDogs)
        {
            if(!(dog.IsInWorld || dog.IsScouting))
            {
                available.Add(dog);
            }
        }
        return available.ToArray();
    }

    TimeSpan getTimePlayedInSession()
    {
        if(this.lastTimeLoaded == default(DateTime))
        {
            return TimeSpan.Zero;
        }
        else
        {
            return DateTime.Now - this.lastTimeLoaded;
        }
    }

}
