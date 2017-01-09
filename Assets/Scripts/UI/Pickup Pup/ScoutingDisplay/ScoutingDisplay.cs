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
		setupScoutingSlots(scoutingSlots);
	}

	#endregion 

	void setupScoutingSlots(DogOutsideSlot[] slots)
	{
		foreach(DogOutsideSlot slot in slots) 
		{
			slot.SubscribeToClickWhenFree(handleClickFreeSlot);
			slot.SubscribeToUIButton();
		}
	}

	void handleClickFreeSlot() 
	{
		dogBrowser.Open();
	}

}
