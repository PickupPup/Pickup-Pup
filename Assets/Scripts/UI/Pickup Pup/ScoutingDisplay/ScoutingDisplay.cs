/*
 * Author(s): Isaiah Mann
 * Description: Controls the logic of displaying dogs scounting
 * Usage: [no notes]
 */

using UnityEngine;

public class ScoutingDisplay : PPUIElement 
{
	[SerializeField]
	DogBrowser dogBrowser;
	DogOutsideSlot[] scoutingSlots;

	#region Override MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		scoutingSlots = GetComponentsInChildren<DogOutsideSlot>();
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		game = PPGameController.GetInstance;
		setupScoutingSlots(scoutingSlots);
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
			slot.SubscribeToUIButton();
		}
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

}
