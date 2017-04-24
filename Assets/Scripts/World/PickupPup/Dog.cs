/*
 * Author: Timothy Ng, Isaiah Mann
 * Description: Controls a dog in the game world
 */

using UnityEngine;
using k = PPGlobal;

public class Dog : MobileObjectBehaviour 
{
	#region Instance Accessors

	public bool IsScouting 
	{
		get 
		{
            checkReferences();
            return dataController.CheckIsScouting(this);
		}
	}

	public float RemainingTimeScouting 
	{
		get 
		{
			if(HasScoutingTimer) 
			{
				return scoutingTimer.TimeRemaining;
			}
			else 
			{
				return default(float);
			}
		}
	}

	public float TotalTimeToReturn
	{
		get
		{
			return Info.TotalTimeToReturn;
		}
	}

	public string RemainingTimeScoutingStr
	{
		get 
		{
			if(HasScoutingTimer)
			{
				return scoutingTimer.TimeRemainingStr;
			}
			else 
			{
				return PPTimer.DefaultTimeRemainingStr;	
			}
		}
	}

	public bool HasScoutingTimer 
	{
		get 
		{
			return scoutingTimer != null;
		}
	}


	public string Name 
	{
		get 
		{
			return descriptor.Name;
		}
	}

	public DogDescriptor Info
	{
		get
		{
			return this.descriptor;
		}
	}

	public Sprite Portrait
	{
		get
		{
			if(hasDescriptor)
			{
				return descriptor.Portrait;
			}
			else 
			{
				return DogDatabase.DefaultSprite;
			}
		}
	}

    public Sprite WorldSprite
    {
        get
        {
            if(hasDescriptor)
            {
                return descriptor.WorldSprite;
            }
            else
            {
                return DogDatabase.DefaultSprite;
            }
        }
    }

	public int ScoutingIndex
	{
		get
		{
			return descriptor.ScoutingSlotIndex;
		}
	}

    public PPScene MostRecentRoom
    {
        get
        {
            return descriptor.MostRecentRoom;
        }
    }

    public bool IsInWorld
    {
        get
        {
            return descriptor.IsInWorld;
        }
    }

	public bool HasSlot
	{
		get
		{
			return slot != null;
		}
	}

	public DogSlot OccupiedSlot
	{
		get
		{
			return slot;
		}
	}

    public string TimeRemainingStr
    {
        get
        {
            return scoutingTimer.TimeRemainingStr;
        }
    }

    public bool HasRedeemableGift
    {
        get
        {
            return redeemableGift != null;   
        }
    }

    // NOTE: Should not be used to actually redeem gift, 
    // --> SEE public CurrenyData RedeemGift() below
    public CurrencyData PeekAtGift
    {
        get
        {
            return redeemableGift;
        }
    }

	#endregion

	bool hasDescriptor 
	{
		get 
		{
			return descriptor != null;
		}
	}

    CurrencyData redeemableGift
    {
        get
        {
            return Info.RedeemableGift;
        }
    }

	// Tracks how long the dog will be away from the house
	[SerializeField]
	protected PPTimer scoutingTimer;

	DogDescriptor descriptor;
	PPData.DogAction onScoutingTimerEnd;
	PPData.DogActionf onScoutingTimerChange;
    PPData.NamedCurrencyAction onGiftAction;
	DogSlot slot;

    #region MonoBehaviourExtended Overrides

    public override bool TryUnsubscribeAll()
    {
        base.TryUnsubscribeAll();
        (scoutingTimer as ISubscribable).TryUnsubscribeAll();
        onScoutingTimerChange = null;
        onScoutingTimerEnd = null;
        return true;
    }

    #endregion

	#region Event Subscription

	public void SubscribeToScoutingTimerEnd(PPData.DogAction dogAction) 
	{
		onScoutingTimerEnd += dogAction;	
	}
		
	public void SubscribeToScoutingTimerEnd(PPData.DataAction dataAction) 
	{
		if(HasScoutingTimer) 
		{
			scoutingTimer.SubscribeToTimeUp(dataAction);	
		}
	}

	public void SubscribeToScoutingTimerChange(PPData.DogActionf dogAction) 
	{
		onScoutingTimerChange += dogAction;
	}

	public void SubscribeToScoutingTimerChange(PPData.DataActionf dataAction) 
	{
		if(HasScoutingTimer) 
		{
			scoutingTimer.SubscribeToTimeChange(dataAction);
		}
	}

	public void UnsubscribeFromScoutingTimerEnd(PPData.DogAction dogAction) 
	{
		onScoutingTimerEnd -= dogAction;	
	}

	public void UnsubscribeFromScoutingTimerEnd(PPData.DataAction dataAction) 
	{
		if(HasScoutingTimer) 
		{
			scoutingTimer.UnsubscribeFromTimeUp(dataAction);	
		}
	}

	public void UnsubscribeFromScoutingTimerChange(PPData.DogActionf dogAction)
	{
		onScoutingTimerChange -= dogAction;
	}


	public void UnsubscribeFromScoutingTimerChange(PPData.DataActionf dataAction) 
	{
		if(HasScoutingTimer) 
		{
			scoutingTimer.UnsubscribeFromTimeChange(dataAction);
		}
	}
   
	public void AssignSlot(DogSlot slot)
	{
		this.slot = slot;
	}

    // WARNING: Do not set callback to true from w/in the slot, otherwise you risk creating stackoverflow
    public void LeaveCurrentSlot(bool callback, bool stopScouting)
	{
        if(stopScouting)
        {
            dataController.ScoutingDogs.Remove(this.Info);
        }
        if(callback && slot)
        {
            this.slot.ClearSlot();
        }
        this.slot = null;
	}

	void callOnScoutingTimerChange(float timer) 
	{
		if(onScoutingTimerChange != null) 
		{
			onScoutingTimerChange(this, timer);
		}
	}

	void callOnScountingTimerEnd()
	{
		if(onScoutingTimerEnd != null) 
		{
			onScoutingTimerEnd(this);
		}
	}

	#endregion

    public void SetTimer(float newTime)
    {
        scoutingTimer.SetTimeRemaining(newTime, checkForEvents:true);
    }

    public void ResumeTimer()
    {
        scoutingTimer.Resume();
    }

    public void StopTimer()
    {
        scoutingTimer.Stop();
    }

    public void IncreaseAffection()
    {
        Info.IncreaseAffection();
        trySaveGame();
    }

    // Typically dog should find a random gift, but method can be overloaded to cause it to find a specific gift
    public void FindGift(bool shouldSave, CurrencyData giftOverride = null)
    {
        CurrencyData gift;
        if(giftOverride == null)
        {
            gift = gameController.GetGift(Info);
        }
        else
        {
            gift = giftOverride;
        }
        Info.FindGift(gift);
        callGiftEvent(k.FIND_GIFT, redeemableGift);
        if(shouldSave)
        {
            trySaveGame();
        }
    }
      
    public CurrencyData RedeemGift()
    {
        if(!HasRedeemableGift)
        {
            FindGift(shouldSave:false);
        }
        CurrencyData gift = Info.RedeemGift();
        callGiftEvent(k.REDEEM_GIFT, gift);
        gameController.GiveCurrency(gift);
		if(gift is SpecialGiftData)
        {
			gift.Give();
        }
        trySaveGame();
        return gift;
    }

	public void Set(DogDescriptor descriptor) 
	{
		this.descriptor = descriptor;
		this.descriptor.LinkToDog(this);
	}

    public void EnterRoom(PPScene room)
    {
        this.descriptor.EnterRoom(room);
    }

    public void LeaveRoom()
    {
        this.descriptor.LeaveRoom();
    }

	protected override void setReferences()
	{
		base.setReferences();
		if(HasScoutingTimer)
		{
			scoutingTimer.Init();
			setupTimer(scoutingTimer);
		}
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		gameController = PPGameController.GetInstance;
        dataController = PPDataController.GetInstance;
	}

	protected override void cleanupReferences()
	{
		base.cleanupReferences();
		if(hasDescriptor) 
		{
			this.descriptor.UnlinkFromDog();
		}
	}

	public void SetGame(PPGameController game)
	{
		this.gameController = game;
	}

	public bool TrySendToScout()
	{
		if(!IsScouting && HasScoutingTimer) 
		{
			descriptor.HandleScoutingBegan(gameController.GetCurrentSlotIndex());
			scoutingTimer.Begin();
			int slotIndex;
			bool wasSuccess = gameController.TrySendDogToScout(this, out slotIndex);
			if(!wasSuccess)
			{
				descriptor.HandleScoutingEnded();
			}
			return wasSuccess;
		} 
		else 
		{
			return false;
		}
	}

	public void GiveTimer(PPTimer timer)
	{
		this.scoutingTimer = timer;
		setupTimer(timer);
	}

    public void SubscribeToGiftEvents(PPData.NamedCurrencyAction currencyAction)
    {
        onGiftAction += currencyAction;
    }

    public void UnsubscribeFromGiftEvents(PPData.NamedCurrencyAction currencyAction)
    {
        onGiftAction -= currencyAction;
    }

	void setupTimer(PPTimer timer)
	{
		timer.SubscribeToTimeChange(callOnScoutingTimerChange);
		timer.SubscribeToTimeUp(callOnScountingTimerEnd);
	}

    void callGiftEvent(string eventName, CurrencyData gift)
    {
        if(onGiftAction != null)
        {
            onGiftAction(eventName, gift);
        }
    }

	public override bool Equals(object obj)
	{
		if(obj is Dog)
		{
			Dog other = obj as Dog;
			return this.Info.Equals(other.Info);
		}
		else
		{
			return base.Equals(obj);
		}
	}

    public override int GetHashCode ()
    {
        return Info.GetHashCode();
    }

}
