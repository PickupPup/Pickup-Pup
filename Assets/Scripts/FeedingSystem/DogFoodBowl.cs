/*
 * Author: Timothy Ng
 * Description: Handles the feeding dog code through calling FeedDogs
 */

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
    Button buttonReference;

    #region MonoBehaviourExtended Overrides 

    protected override void fetchReferences() {
        base.fetchReferences();
        if(feedingTimer == null)
        {
            feedingTimer = new PPTimer(PPGameController.GetInstance.Tuning.DogFoodFeedTimeSec, 1f);
            feedingTimer.SetTimeRemaining(0, false);
        }
        feedingTimer.SubscribeToTimeUp(handleFeedingTimeUp);
        buttonReference = GetComponent<Button>();
        buttonReference.interactable = !IsCurrentlyFeeding;
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        feedingTimer.UnsubscribeFromTimeUp(handleFeedingTimeUp);
    }

    #endregion

    int calculateDogFoodNeeded()
    {
        return dataController.DogCount - dataController.ScoutingDogs.Count;
    }

    public void FeedDogs()
    {
        if(dataController.CanAfford(CurrencyType.DogFood, calculateDogFoodNeeded()) && !IsCurrentlyFeeding)
        {
            dataController.ChangeFood(-calculateDogFoodNeeded());
            feedingTimer.Reset();
            feedingTimer.Begin();
            buttonReference.interactable = false;
        }
    }

    void handleFeedingTimeUp()
    {
        buttonReference.interactable = true;
    }

}
