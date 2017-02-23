/*
 * Author(s): Isaiah Mann
 * Description: Controls the logic of displaying dogs scounting
 * Usage: [no notes]
 */

using UnityEngine;
using System.Collections.Generic;

public class ScoutingDisplay : PPUIElement 
{
    #region Static Accessors

    // Not quite a singleton, because the scouting display is distinct to each scene
    public static ScoutingDisplay MostRecentInstance
    {
        get;
        private set;
    }

    public static bool HasActiveInstance
    {
        get
        {
            return MostRecentInstance != null && MostRecentInstance.gameObject != null;
        }
    }

    #endregion

	[SerializeField]
	DogBrowser dogBrowser;
    [SerializeField]
    RedeemDisplay giftRedeemDisplay;
    [SerializeField]
	GiftReportUI scoutingReportDisplay;

	DogCollarSlot[] scoutingSlots;
	Dictionary<int, DogCollarSlot> slotsByIndex = new Dictionary<int, DogCollarSlot>();

	public void SendToSlot(Dog dog, int slotIndex)
	{
		DogCollarSlot slot;
		if(slotsByIndex.TryGetValue(slotIndex, out slot))
		{
			slot.ResumeScouting(dog);
		}
	}

	public void SubscribeToTimerEnd(Dog dog)
	{
		dog.SubscribeToScoutingTimerEnd(handleScoutingTimerEnd);
	}

    public void UnsubscribeFromTimerEnd(Dog dog)
    {
        dog.UnsubscribeFromScoutingTimerEnd(handleScoutingTimerEnd);
    }

    public bool TryFindOpenSlot(out DogSlot openSlot)
    {
        for(int i = 0; i < scoutingSlots.Length; i++)
        {
            if(!scoutingSlots[i].HasDog)
            {
                openSlot = scoutingSlots[i];
                return true;
            }
        }
        openSlot = null;
        return false;
    }

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		scoutingSlots = GetComponentsInChildren<DogCollarSlot>();
		foreach(DogCollarSlot slot in scoutingSlots)
		{
			slotsByIndex.Add(slot.transform.GetSiblingIndex(), slot);
		}
        MostRecentInstance = this;
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		gameController = PPGameController.GetInstance;
		setupScoutingSlots(scoutingSlots);
	}

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		EventController.Subscribe(handlePPDogEvent);
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		EventController.Unsubscribe(handlePPDogEvent);
        unsubscribeFromAllDogs();
	}
        
	#endregion 

    void unsubscribeFromAllDogs()
    {
        foreach(DogCollarSlot scoutingSlot in scoutingSlots)
        {
            if(scoutingSlot.HasDog)
            {
                scoutingSlot.PeekDog.UnsubscribeFromScoutingTimerEnd(handleScoutingTimerEnd);
            }
        }
    }

	void setupScoutingSlots(DogCollarSlot[] slots)
	{
		foreach(DogCollarSlot slot in slots) 
		{
			slot.SubscribeToClickWhenFree(
				delegate
                {
					gameController.SetTargetSlot(slot);
					handleClickFreeSlot();
				}
			);
            slot.SubscribeToClickWhenOccupied(
                delegate 
                {
					if(slot.PeekDog.HasRedeemableGift)
					{
                    	setupRedeemDisplay(slot.PeekDog);
                    	slot.SetText(string.Empty);
                        slot.ToggleRedeemDisplayOpen(isOpen:true);
					}
                }
            );
		}
	}

	void handleScoutingTimerEnd(Dog dog)
	{
        // Safeguard to prevent multiple copies of this method being subscribed:
        dog.UnsubscribeFromScoutingTimerEnd(handleScoutingTimerEnd);
        dog.FindGift(shouldSave:true);
        dog.Info.HandleScoutingEnded();
	}

    void handleDogGiftCollected(Dog dog, bool resendOutToScout)
    {
        if(gameController)
        {
            DogDescriptor dogInfo = dog.Info;
            CurrencyData reward = gameController.GetGift(dogInfo);
            GiftReport report = new GiftReport(dogInfo, reward);
            createGiftReportUI(report);
        }
    }

    void setupRedeemDisplay(Dog dog)
    {
        RedeemDisplay redeemDisplay = createRedeemDisplay(dog); 
		redeemDisplay.Init(dog);
    }

    RedeemDisplay createRedeemDisplay(Dog dog)
    {
        UIElement elem;
        if(!UIElement.TryPullFromSpawnPool(typeof(RedeemDisplay), out elem))
        {
            elem = Instantiate<UIElement>(giftRedeemDisplay);
        }
        return elem as RedeemDisplay;
    }

	GiftReportUI createGiftReportUI(GiftReport report)
	{
		GiftReportUI reportUI = Instantiate(scoutingReportDisplay);
		reportUI.Init(report);
		return reportUI;
	}

	GiftReport getScoutingReport(Dog dog, CurrencyData reward)
	{
		return new GiftReport(dog.Info, reward);
	}
        
    void refreshScoutingDogs()
    {
        dataController.ClearScoutingDogs();
        foreach(DogCollarSlot slot in scoutingSlots)
        {
            if(slot.HasDog)
            {
                dataController.SendDogToScout(slot.PeekDog);
            }
        }
    }

	void handleClickFreeSlot() 
	{
        refreshScoutingDogs();
		dogBrowser.Open(inScoutingSelectMode:true);
		dogBrowser.SubscribeToDogClick(handleDogSelected);
	}

	void handleDogSelected(Dog dog)
	{
		gameController.SendToTargetSlot(dog);
		dogBrowser.UnsubscribeFromDogClick(handleDogSelected);
		dogBrowser.Hide();
	}

	void handlePPDogEvent(PPEvent eventName, Dog dog)
	{
		if(eventName == PPEvent.ScoutingDogLoaded)
		{
			SendToSlot(dog, dog.ScoutingIndex);
		}
	}

}
