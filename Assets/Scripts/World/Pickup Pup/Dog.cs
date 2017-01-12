/*
 * Author: Isaiah Mann
 * Description: Controls a dog in the game world
 */

using UnityEngine;

public class Dog : MobileObjectBehaviour 
{
	#region Instance Accessors

	public bool IsScouting 
	{
		get 
		{
			return RemainingTimeScouting > 0 && scoutingTimer.IsRunning;
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

	public int ScoutingIndex
	{
		get
		{
			return descriptor.ScoutingSlotIndex;
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

	#endregion

	bool hasDescriptor 
	{
		get 
		{
			return descriptor != null;
		}
	}

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

	// Tracks how long the dog will be away from the house
	[SerializeField]
	protected PPTimer scoutingTimer;

	DogDescriptor descriptor;
	PPGameController game;
	PPData.DogAction onScoutingTimerEnd;
	PPData.DogActionf onScoutingTimerChange;
	DogSlot slot;

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

	public void LeaveCurrentSlot()
	{
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

	public void Set(DogDescriptor descriptor) 
	{
		this.descriptor = descriptor;
		this.descriptor.LinkToDog(this);
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
		game = PPGameController.GetInstance;
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
		this.game = game;
	}

	public bool TrySendToScout()
	{
		if(!IsScouting && HasScoutingTimer) 
		{
			descriptor.HandleScoutingBegun(game.GetCurrentSlotIndex());
			scoutingTimer.Begin();
			int slotIndex;
			bool wasSuccess = game.TrySendDogToScout(this, out slotIndex);
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

	void setupTimer(PPTimer timer)
	{
		timer.SubscribeToTimeChange(callOnScoutingTimerChange);
		timer.SubscribeToTimeUp(callOnScountingTimerEnd);
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

}
