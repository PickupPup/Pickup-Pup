/*
 * Authors: Timothy Ng, Isaiah Mann
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
    Sprite dogFoodBowlImage;

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
        dogFoodBowlImage = gameObject.GetComponent<Button>().spriteState.disabledSprite;
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

    public void FeedDogs(bool isSpecial)
    {
		int foodNeeded = calculateDogFoodNeeded();
		if(foodNeeded <= 0)
		{
			return;
		}

		if(dataController.CanAfford(CurrencyType.DogFood, foodNeeded) && !IsCurrentlyFeeding)
        {
            dataController.ChangeFood(-calculateDogFoodNeeded(), isSpecial);
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
    
    public void ToggleFoodOptions(bool isSpecial)
    {
        Debug.Log("Toggle");
        // if we are selecting a food option
        if (!foodOptions.activeSelf == false)
        {
            Debug.Log("Turning off, selected food");
            if (!isSpecial)
            {
                Debug.Log("Selected Regular");
                //dogFoodBowlImage = redFilledBowlSprite
            }
            else
            {
                Debug.Log("Selected Special");
                //dogFoodBowlImage = blueFilledBowlSprite
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
