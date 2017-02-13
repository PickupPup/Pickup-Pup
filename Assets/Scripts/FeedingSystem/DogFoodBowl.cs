using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFoodBowl : MonoBehaviour {

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

    void Start () {
        if(feedingTimer == null)
        {
            feedingTimer = new PPTimer(20, 1f);
            feedingTimer.SetTimeRemaining(0, false);
        }
    }

    int calculateDogFoodNeeded()
    {
        return PPDataController.GetInstance.DogCount - PPDataController.GetInstance.ScoutingDogs.Count;
    }

    public void feedDogs()
    {
        if (PPDataController.GetInstance.CanAfford(CurrencyType.DogFood, calculateDogFoodNeeded()) && !IsCurrentlyFeeding)
        {
            PPDataController.GetInstance.ChangeFood(-calculateDogFoodNeeded());
            feedingTimer.SetTimeRemaining(20, false);
            feedingTimer.Begin();
        }
    }
}
