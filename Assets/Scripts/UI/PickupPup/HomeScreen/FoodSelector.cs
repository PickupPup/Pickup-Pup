/*
 * Author(s): Isaiah Mann
 * Description: Select a type of food to use
 * Usage: [no notes]
 */

using System;
using UnityEngine;
using k = PPGlobal;

public class FoodSelector : PPUIElement
{
	[SerializeField]
	PPUIButton previousButton;
	[SerializeField]
	PPUIButton nextButton;
	[SerializeField]
	PPUIButton yesButton;
	[SerializeField]
	PPUIButton noButton;
	[SerializeField]
	PPUIButton closeButton;

	[SerializeField]
	PPUIElement foodBowlIcon;
	[SerializeField]
	PPUIElement foodAmount;
	[SerializeField]
	PPUIElement descriptionDisplay;

	DogFoodData[] foodOptions;
	DogFoodData selectedFood;
	int currentFoodIndex;
	Action<DogFoodData> onClickYes;
	Action<DogFoodData> defaultClickYes;

	#region MonoBehaviourExtended Overrides

	protected override void fetchReferences()
	{
		base.fetchReferences();
		setupButtons();
	}

	#endregion

	#region UIElementOverrides

	public override void Show()
	{
		base.Show();
		updateFoodDisplay(selectedFood);
	}

	#endregion

	public void Setup(FoodSystem food, Action<DogFoodData> onClickYes)
	{
		checkReferences();
		foodOptions = food.GetAvailableFoods();
		setFood(food.GetLastUsedFood());
		checkToHideButtons();
		SetDelegate(onClickYes);
		this.defaultClickYes = onClickYes;
	}

	public void SetDelegate(Action<DogFoodData> onClickYes)
	{
		this.onClickYes = onClickYes;
	}

	public void SetDescription(string description)
	{
		descriptionDisplay.SetText(description);
	}

	void setupButtons()
	{
		previousButton.SubscribeToClick(previousFood);
		nextButton.SubscribeToClick(nextFood);
		yesButton.SubscribeToClick(callOnClickYes);
		noButton.SubscribeToClick(Hide);
		closeButton.SubscribeToClick(Hide);
	}

	void callOnClickYes()
	{
		if(onClickYes != null)
		{
			onClickYes(selectedFood);
		}
	}
		
	void checkToHideButtons()
	{
		if(foodOptions.Length <= k.SINGLE_VALUE)
		{
			previousButton.Hide();
			nextButton.Hide();
		}
	}

	void previousFood()
	{
		currentFoodIndex--;
		if(currentFoodIndex < k.NONE_VALUE)
		{
			currentFoodIndex = foodOptions.Length - k.SINGLE_VALUE;
		}
		setFood(foodOptions[currentFoodIndex], calculateIndex:false);
	}

	void nextFood()
	{
		currentFoodIndex++;
		currentFoodIndex %= foodOptions.Length;
		setFood(foodOptions[currentFoodIndex], calculateIndex:false);
	}
		
	void setFood(DogFoodData food, bool calculateIndex = true)
	{
		selectedFood = food;
		if(calculateIndex)
		{
			try
			{
				currentFoodIndex = ArrayUtil.IndexOf(foodOptions, selectedFood);
			}
			catch
			{
				currentFoodIndex = k.NONE_VALUE;
				selectedFood = foodOptions[currentFoodIndex];
			}
		}
		updateFoodDisplay(food);
	}

	void updateFoodDisplay(DogFoodData food)
	{
		foodBowlIcon.SetImageColor(food.Color);
		foodAmount.SetText(food.Amount.ToString());
		descriptionDisplay.SetText(food.Description);
		SetDelegate(defaultClickYes);
	}

}
