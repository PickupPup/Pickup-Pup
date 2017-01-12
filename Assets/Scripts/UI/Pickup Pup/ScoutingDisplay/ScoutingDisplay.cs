/*
 * Author(s): Isaiah Mann
 * Description: Controls the logic of displaying dogs scounting
 * Usage: [no notes]
 */

using UnityEngine;
using System.Collections.Generic;

public class ScoutingDisplay : PPUIElement 
{
	[SerializeField]
	DogBrowser dogBrowser;
	[SerializeField]
	ScoutingReportUI scoutingReportDisplay;

	DogOutsideSlot[] scoutingSlots;
	Dictionary<int, DogOutsideSlot> slotsByIndex = new Dictionary<int, DogOutsideSlot>();

	public void SendToSlot(Dog dog, int slotIndex)
	{
		DogOutsideSlot slot;
		if(slotsByIndex.TryGetValue(slotIndex, out slot))
		{
			slot.ResumeScouting(dog);
		}
	}

	public void SubscribeToTimerEnd(Dog dog)
	{
		dog.SubscribeToScoutingTimerEnd(handleScoutingTimerEnd);
	}

	#region Override MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		scoutingSlots = GetComponentsInChildren<DogOutsideSlot>();
		foreach(DogOutsideSlot slot in scoutingSlots)
		{
			slotsByIndex.Add(slot.transform.GetSiblingIndex(), slot);
		}
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		game = PPGameController.GetInstance;
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
				delegate() {
					game.SetTargetSlot(slot);
					handleClickFreeSlot();		
				}
			);
		}
	}

	void handleScoutingTimerEnd(Dog dog)
	{
		DogDescriptor dogInfo = dog.Info;
		CurrencyData reward = game.GetGift(dogInfo);
		ScoutingReport report = new ScoutingReport(dogInfo, reward);
		scoutingReportDisplay.Init(report);
	}

	ScoutingReport getScoutingReport(Dog dog, CurrencyData reward)
	{
		return new ScoutingReport(dog.Info, reward);
	}

	void handleClickFreeSlot() 
	{
		dogBrowser.Open();
		dogBrowser.SubscribeToDogClick(handleDogSelected);
	}

	void handleDogSelected(Dog dog)
	{
		game.SendToTargetSlot(dog);
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
