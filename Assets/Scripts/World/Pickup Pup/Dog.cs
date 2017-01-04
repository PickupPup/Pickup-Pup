/*
 * Author: Isaiah Mann
 * Description: Controls a dog in the game world
 */

using UnityEngine;

public class Dog : MobileObjectBehaviour {
	PPData.DogAction onScoutingTimerEnd;
	PPData.DogActionf onScoutingTimerChange;

	public bool IsScouting {
		get {
			return RemainingTimeScouting > 0;
		}
	}
	public float RemainingTimeScouting {
		get {
			if (HasScoutingTimer) {
				return scoutingTimer.TimeRemaining;
			} else {
				return default(float);
			}
		}
	}
	PPGameController game;

	public bool HasScoutingTimer {
		get {
			return scoutingTimer != null;
		}
	}

	[SerializeField]
	// Tracks how long the dog will be away from the house
	protected PPTimer scoutingTimer;
	DogDescriptor descriptor;
	bool hasDescriptor {
		get {
			return descriptor != null;
		}
	}

	public string Name {
		get {
			return descriptor.Name;
		}
	}

	#region Event Subscription

	public void SubscribeToScoutingTimerEnd (PPData.DogAction dogAction) {
		onScoutingTimerEnd += dogAction;	
	}
		
	public void SubscribeToScoutingTimerEnd (PPData.DataAction dataAction) {
		if (HasScoutingTimer) {
			scoutingTimer.SubscribeToTimeUp(dataAction);	
		}
	}

	public void SubscribeToScoutingTimerChange (PPData.DogActionf dogAction) {
		onScoutingTimerChange += dogAction;
	}

	public void SubscribeToScoutingTimerChange (PPData.DataActionf dataAction) {
		if (HasScoutingTimer) {
			scoutingTimer.SubscribeToTimeChange(dataAction);
		}
	}

	public void UnsubscribeFromScoutingTimerEnd (PPData.DogAction dogAction) {
		onScoutingTimerEnd -= dogAction;	
	}

	public void UnsubscribeFromScoutingTimerEnd (PPData.DataAction dataAction) {
		if (HasScoutingTimer) {
			scoutingTimer.UnsubscribeFromTimeUp(dataAction);	
		}
	}

	public void UnsubscribeFromScoutingTimerChange (PPData.DogActionf dogAction) {
		onScoutingTimerChange -= dogAction;
	}


	public void UnsubscribeFromScoutingTimerChange (PPData.DataActionf dataAction) {
		if (HasScoutingTimer) {
			scoutingTimer.UnsubscribeFromTimeChange(dataAction);
		}
	}

	void callOnScoutingTimerChange (float timer) {
		if (onScoutingTimerChange != null) {
			onScoutingTimerChange(this, timer);
		}
	}

	void callOnScountingTimerEnd () {
		if (onScoutingTimerEnd != null) {
			onScoutingTimerEnd(this);
		}
	}

	#endregion

	public void Set (DogDescriptor descriptor) {
		this.descriptor = descriptor;
		this.descriptor.LinkToDog(this);
	}

	protected override void SetReferences () {
		base.SetReferences ();
		scoutingTimer.Init();
	}

	protected override void FetchReferences () {
		base.FetchReferences ();
		game = PPGameController.GetInstance;
	}

	protected override void CleanupReferences () {
		base.CleanupReferences ();
		if (hasDescriptor) {
			this.descriptor.UnlinkFromDog();
		}
	}

	public bool TrySendToScout () {
		if (!IsScouting && HasScoutingTimer) {
			scoutingTimer.Begin();
			return game.TrySendDogToScout(this);
		} else {
			return false;
		}
	}
}
