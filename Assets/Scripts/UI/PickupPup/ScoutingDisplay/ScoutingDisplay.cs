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

	DogOutsideSlot[] scoutingSlots;
	Dictionary<int, DogOutsideSlot> slotsByIndex = new Dictionary<int, DogOutsideSlot>();

	public void SendToSlot(Dog dog, int slotIndex)
	{
		DogOutsideSlot slot;
		if(slotsByIndex.TryGetValue(slotIndex, out slot))
		{
			slot.ResumeScouting(dog);
            slot.SubscribeToClickWhenOccupied(
                delegate 
                {
                    if(slot.PeekDog.HasRedeemableGift)
                    {
                        setupRedeemDisplay(slot.PeekDog);
                        slot.SetText(string.Empty);
                    }
                }
            );
		}
	}

	public void SubscribeToTimerEnd(Dog dog)
	{
		dog.SubscribeToScoutingTimerEnd(handleScoutingTimerEnd);
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		scoutingSlots = GetComponentsInChildren<DogOutsideSlot>();
		foreach(DogOutsideSlot slot in scoutingSlots)
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
	}

	#endregion 

	void setupScoutingSlots(DogOutsideSlot[] slots)
	{
		foreach(DogOutsideSlot slot in slots) 
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
					}
                }
            );
		}
	}

	void handleScoutingTimerEnd(Dog dog)
	{
        // Safeguard to prevent multiple copies of this method being subscribed:
        dog.UnsubscribeFromScoutingTimerEnd(handleScoutingTimerEnd);
        dog.FindGift();
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

	void handleClickFreeSlot() 
	{
		dogBrowser.Open();
		dogBrowser.SubscribeToDogClick(handleDogSelected);
	}

	void handleDogSelected(Dog dog)
	{
		gameController.SendToTargetSlot(dog);
		dogBrowser.UnsubscribeFromDogClick(handleDogSelected);
		dogBrowser.Close();
	}

	void handlePPDogEvent(PPEvent eventName, Dog dog)
	{
		if(eventName == PPEvent.ScoutingDogLoaded)
		{
			SendToSlot(dog, dog.ScoutingIndex);
		}
	}

}
