using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedingController : SingletonController<FeedingController> {

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

    [SerializeField]
    PPTimer feedingTimer;

    int calculateDogFoodNeeded()
    {
        return PPDataController.GetInstance.DogCount - PPDataController.GetInstance.ScoutingDogs.Count;
    }

    public bool TryFeedDogs()
    {
        if (PPDataController.GetInstance.CanAfford(CurrencyType.DogFood, calculateDogFoodNeeded()) && !IsCurrentlyFeeding)
        {
            feedDogs();
            return true;
        }
        return false;
    }

    void feedDogs()
    {
        dataController.ChangeFood(-calculateDogFoodNeeded());
        feedingTimer.SetTimeRemaining(20, false);

        //TODO implement sprite change
    }

}
