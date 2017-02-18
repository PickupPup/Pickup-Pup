using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DogFoodBowl : MonoBehaviourExtended
{

    #region Instance Accessors

    public float FeedTimerLeft
    {
        get
        {
            return feedingTimer.TimeRemaining;
        }
    }

    public bool IsCurrentlyFeeding
    {
        get
        {
            return feedingTimer.TimeRemaining != 0;
        }
    }

    #endregion

    static PPTimer feedingTimer = null;

    protected override void fetchReferences() {
        base.fetchReferences();

        if (feedingTimer == null)
        {
            feedingTimer = new PPTimer(20, 1f);
            feedingTimer.SetTimeRemaining(0, false);
        }
        feedingTimer.SubscribeToTimeUp(handleFeedingTimeUp);
        GetComponent<Button>().interactable = !IsCurrentlyFeeding;
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        feedingTimer.UnsubscribeFromTimeUp(handleFeedingTimeUp);
    }

    int calculateDogFoodNeeded()
    {
        return PPDataController.GetInstance.DogCount - PPDataController.GetInstance.ScoutingDogs.Count;
    }

    public void FeedDogs()
    {
        if (PPDataController.GetInstance.CanAfford(CurrencyType.DogFood, calculateDogFoodNeeded()) && !IsCurrentlyFeeding)
        {
            PPDataController.GetInstance.ChangeFood(-calculateDogFoodNeeded());
            feedingTimer.SetTimeRemaining(20, true);
            feedingTimer.Begin();
            GetComponent<Button>().interactable = false;
        }
    }

    void handleFeedingTimeUp()
    {
        GetComponent<Button>().interactable = true;
    }
}
