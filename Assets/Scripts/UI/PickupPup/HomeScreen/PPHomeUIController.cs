/*
 * Author: Grace Barrett-Snyder
 * Description: Controls the Home UI (Living Room and Yard).
 */

using UnityEngine;

public class PPHomeUIController : PPUIController
{
    DogWorldSlot[] dogWorldSlots;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
		if(PlayerPrefsUtil.ShowedFirstHomePrompt)
		{
			promptID = PromptID.ScoutingPrompt;
		}
		else
		{
			promptID = PromptID.FirstHomePrompt;
		}
        dogWorldSlots = GetComponentsInChildren<DogWorldSlot>();
        base.setReferences();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadHome);
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        foreach(DogWorldSlot dogSlot in dogWorldSlots)
        {
            dogSlot.SubscribeToNameTagClick(handleDogSlotClicked);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        foreach(DogWorldSlot dogSlot in dogWorldSlots)
        {
            dogSlot.UnsubscribeFromNameTagClick(handleDogSlotClicked);
        }
    }

    #endregion

    #region PPUIController Overrides

    protected override void showPopupPrompt()
    {
		if(!PlayerPrefsUtil.ShowedFirstHomePrompt)
		{
			base.showPopupPrompt();
			PlayerPrefsUtil.ShowedFirstHomePrompt = true;
		}	
        else if(!PlayerPrefsUtil.ShowedScoutingPrompt)
        {
            base.showPopupPrompt();
            PlayerPrefsUtil.ShowedScoutingPrompt = true;
        }
    }

    #endregion

}
