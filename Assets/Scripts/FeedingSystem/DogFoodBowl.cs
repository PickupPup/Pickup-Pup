/*
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
        int isSpecialIncrement = System.Convert.ToInt32(isSpecial);
        int foodNeeded = calculateDogFoodNeeded();
		if(foodNeeded <= 0)
		{
			return;
		}

		if(dataController.CanAfford(CurrencyType.DogFood + isSpecialIncrement, foodNeeded) && !IsCurrentlyFeeding)
        {
            dataController.ChangeFood(-calculateDogFoodNeeded(), foodType);
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

    /* BP: It might be needed to access different colored bowls to illustrate
     * which food is currently being eaten (swap the sprite of the dogfoodbutton).
     * I imagine it would be done here.
     */
    public void ToggleFoodOptions(bool isSpecial)
    {
        // if we are selecting a food option
        if (!foodOptions.activeSelf == false)
        {
            if (!isSpecial)
            {
                //disabledDogFoodBowlImage = redFilledBowlSprite
            }
            else
            {
                //disabledDogFoodBowlImage = blueFilledBowlSprite
            }
        }
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
