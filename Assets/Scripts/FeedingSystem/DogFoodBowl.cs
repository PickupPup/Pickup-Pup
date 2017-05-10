/*
 * Authors: Timothy Ng, Isaiah Mann, Ben Page
 * Description: Handles the feeding dog code through calling FeedDogs
 */

using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

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
    List<DogDescriptor> dogsToFeed;

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
        dogsToFeed = dataController.AdoptedDogs;
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


    
    DogFoodType foodToFeed;
    public void FeedDogs(int foodType)
    {
        foodToFeed = (DogFoodType)foodType;
        Debug.Log("Feeding with " + (DogFoodType)foodType);
        int foodNeeded = calculateDogFoodNeeded();
        if (foodNeeded <= 0)
		{
			return;
		}

		if(dataController.CanAffordFood(foodToFeed, foodNeeded) && !IsCurrentlyFeeding)
        {
            Debug.Log("!");
            dataController.ChangeFood(-calculateDogFoodNeeded(), (DogFoodType)foodType);

            //Debug.Log("ScoutingDogs Dogs: " + dataController.ScoutingDogs.Count);
            //Debug.Log("AdoptedDogs Dogs: " + dataController.AdoptedDogs.Count);

            // 1. Get all of our dogs
            List<DogDescriptor> scoutingDogs = dataController.ScoutingDogs;

            // 2. Remove any scouting dogs (we cannot feed those)
            for (int i = 0; i < scoutingDogs.Count; i++)
            {
                DogDescriptor currentDog = scoutingDogs[i];
                if (dogsToFeed.Contains(currentDog))
                {
                    dogsToFeed.Remove(currentDog);
                }
            }
            Debug.Log(dogsToFeed + " " + foodToFeed);

            feedingTimer.Reset();
            feedingTimer.Begin();
            buttonReference.interactable = false;
            EventController.Event(k.GetPlayEvent(k.ADD_FOOD));
        }
        else
        {
            EventController.Event(k.GetPlayEvent(k.EMPTY));
        }
    }

    public void FeedDogs2()
    {
        Debug.Log(foodToFeed);
        bool  tempFed = true;
        float tempSpGift = 0.1f;
        float tempGiftRate = 1.0f;
        switch (foodToFeed)
        {
            case DogFoodType.Regular:
                tempFed = true;
                tempSpGift = 0.1f;
                tempGiftRate = 1.0f;
                break;
            case DogFoodType.Super:
                tempFed = true;
                tempSpGift = 0.25f;
                tempGiftRate = 1.0f;
                break;
            case DogFoodType.Mega:
                tempFed = true;
                tempSpGift = 0.25f;
                tempGiftRate = 2.0f;
                break;
            case DogFoodType.Ultra:
                tempFed = true;
                tempSpGift = 0.50f;
                tempGiftRate = 2.0f;
                break;
        }
        for (int i = 0; i < dogsToFeed.Count; i++)
        {
            dogsToFeed[i].IsFed = tempFed;
            dogsToFeed[i].SpGiftChance = tempSpGift;
            dogsToFeed[i].DogGiftRate = tempGiftRate;
            Debug.Log(dogsToFeed[i].Name + " is now fed with " + foodToFeed);
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
        FeedDogs2();
        buttonReference.interactable = true;
    }
}
