﻿/*
 * Authors: Timothy Ng, Isaiah Mann, Ben Page
 * Description: Handles the feeding dog code through calling FeedDogs
 */

using UnityEngine.UI;
using UnityEngine;

using k = PPGlobal;

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

    public float GetFeedingTime
    {
        get
        {
            // Check to prevent stack overflow (if timer instantly completes)
            float tuningTime = gameController.Tuning.DogFoodFeedTimeSec;
            if(tuningTime > NONE_VALUE)
            {
                return tuningTime;
            }
            else
            {
                return SINGLE_VALUE;
            }
        }
    }

    #endregion

    static PPTimer feedingTimer = null;
    Button buttonReference;
    GameObject foodOptions;
    
    #region MonoBehaviourExtended Overrides 

    protected override void fetchReferences() 
    {
        base.fetchReferences();
        if(feedingTimer == null)
        {
            feedingTimer = new PPTimer(GetFeedingTime, 1f);
            feedingTimer.SetTimeRemaining(0, false);
        }
        feedingTimer.SubscribeToTimeBegin(handleFeedingTimeBegin);
        feedingTimer.SubscribeToTimeUp(handleFeedingTimeUp);
        buttonReference = GetComponent<Button>();
        buttonReference.interactable = !IsCurrentlyFeeding;
        foodOptions = transform.GetChild(0).gameObject;
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        feedingTimer.UnsubscribeFromTimeBegin(handleFeedingTimeBegin);
        feedingTimer.UnsubscribeFromTimeUp(handleFeedingTimeUp);
    }

    #endregion

    int calculateDogFoodNeeded()
    {
        return dataController.DogCount - dataController.ScoutingDogs.Count;
    }

    public void FeedDogs(int foodType)
    {
        DogFoodType dogFoodType = (DogFoodType)foodType;
        int foodNeeded = calculateDogFoodNeeded();
        Debug.Log("Feed the puppers " + foodNeeded + " " + dogFoodType + " food, please.");
        if (foodNeeded <= 0)
		{
			return;
		}

		if(dataController.CanAffordFood(dogFoodType, foodNeeded) && !IsCurrentlyFeeding)
        {
            Debug.Log("Feed em!");
            dataController.ChangeFood(-calculateDogFoodNeeded(), (DogFoodType)foodType);
            feedingTimer.Reset();
            feedingTimer.Begin();
            buttonReference.interactable = false;
            EventController.Event(k.GetPlayEvent(k.ADD_FOOD));
        }
        else
        {
            Debug.Log("Either you don't have enough food or they are already eating!");
            EventController.Event(k.GetPlayEvent(k.EMPTY));
        }
    }

    // BP This is an appropriate place to figure out which disabled sprite we are using for the bowl.
    public void ToggleFoodOptions()
    {
        foodOptions.SetActive(!foodOptions.activeSelf);
    }

    void handleFeedingTimeBegin()
    {
        buttonReference.interactable = !IsCurrentlyFeeding;
    }

    void handleFeedingTimeUp()
    {
        buttonReference.interactable = true;
    }

}
