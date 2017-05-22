/*
 * Authors: Timothy Ng, Isaiah Mann
 * Description: Handles the feeding dog code through calling FeedDogs
 */

using UnityEngine.UI;
using UnityEngine;

using k = PPGlobal;

public class DogFoodBowl : MonoBehaviourExtended
{
	static PPTimer feedingTimer = null;
	static DogFoodData currentFood;

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

    Button buttonReference;
	Image foodBowlImage;

	[SerializeField]
	FoodSelector foodSelector;
	[SerializeField]
	PPUIElement dogFoodImage;

    #region MonoBehaviourExtended Overrides 

	protected override void setReferences()
	{
		base.setReferences();
		foodBowlImage = GetComponent<Image>();
	}

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
		foodSelector.Setup(dataController.AllFood, feedDogs);
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        feedingTimer.UnsubscribeFromTimeBegin(handleFeedingTimeBegin);
        feedingTimer.UnsubscribeFromTimeUp(handleFeedingTimeUp);
    }

    #endregion

	public void ChooseFood()
	{
		foodSelector.Show();
	}

	void feedDogs(DogFoodData food)
    {
		int amountFoodNeeded = calculateDogFoodNeeded();
		if(amountFoodNeeded <= k.NONE_VALUE)
		{
			return;
		}
		DogFoodData foodNeeded = food.Copy();
		foodNeeded.SetAmount(-amountFoodNeeded);
		if(dataController.HasFood(food.FoodType, amountFoodNeeded) && !IsCurrentlyFeeding)
        {
			dataController.ChangeCurrencyAmount(foodNeeded);
            buttonReference.interactable = false;
            EventController.Event(k.GetPlayEvent(k.ADD_FOOD));
            dataController.RefillDogFood();
			giveDogsFood(foodNeeded);
			foodBowlImage.color = food.Color;
			currentFood = foodNeeded;
			feedingTimer.Reset();
			feedingTimer.Begin();
        }
        else
        {
            EventController.Event(k.GetPlayEvent(k.EMPTY));
        }
    }

	int calculateDogFoodNeeded()
	{
		return dataController.DogCount - dataController.ScoutingDogs.Count;
	}

	void giveDogsFood(DogFoodData food)
	{
		foreach(DogDescriptor dog in dataController.AvailableDogs)
		{
			DogFoodData pieceOfFood = food.GetPiece();
			dog.EatFood(pieceOfFood);
		}
	}

    void handleFeedingTimeBegin()
    {
        buttonReference.interactable = !IsCurrentlyFeeding;
		dogFoodImage.Show();
		foodBowlImage.color = currentFood.Color;
    }

    void handleFeedingTimeUp()
    {
        buttonReference.interactable = true;
        EventController.Event(k.GetPlayEvent(k.EAT_FOOD));
		dogFoodImage.Hide();
    }

}
