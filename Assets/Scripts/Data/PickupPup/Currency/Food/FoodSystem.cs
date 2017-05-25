/*
 * Author(s): Isaiah Mann
 * Description: Controls the food the player has available
 * Usage: [no notes]
 */

using System.Collections.Generic;
using k = PPGlobal;

[System.Serializable]
public class FoodSystem : PPData
{
	Dictionary<string, DogFoodData> foods = new Dictionary<string, DogFoodData>();
	DogFoodData lastUsedFood;

	public DogFoodData[] GetAvailableFoods()
	{
		List<DogFoodData> availableFoods = new List<DogFoodData>();
		foreach(DogFoodData food in foods.Values)
		{
			if(food.Amount > k.NONE_VALUE)
			{
				availableFoods.Add(food);
			}
		}
		if(availableFoods.Count == k.NONE_VALUE)
		{
			availableFoods.Add(GetFood());
		}
		return availableFoods.ToArray();
	}

	public void AddFood(DogFoodData newFood)
	{
		DogFoodData existingFood;
		if(foods.TryGetValue(newFood.FoodType, out existingFood))
		{
			existingFood.ChangeBy(newFood.Amount);
		}
		else
		{
			foods[newFood.FoodType] = newFood;
		}
	}

	public int GetAmount(string type = k.DEFAULT_FOOD_TYPE)
	{
		return GetFood(type).Amount;
	}

	public DogFoodData GetLastUsedFood()
	{
		if(lastUsedFood == null)
		{
			lastUsedFood = GetFood();
		}
		return lastUsedFood;
	}
		
	public DogFoodData GetFood(string type = k.DEFAULT_FOOD_TYPE)
	{
		DogFoodData match;
		if(foods.TryGetValue(type, out match))
		{
			return match;
		}
		else
		{
			return addNewEntryToFoods(type);
		}
	}

	public void ChangeBy(int delta, string type = k.DEFAULT_FOOD_TYPE)
	{
		DogFoodData food = GetFood(type);
		food.ChangeBy(delta);
		if(delta < k.NONE_VALUE)
		{
			this.lastUsedFood = food;
		}
	}

	public void SetAmount(int amount, string type = k.DEFAULT_FOOD_TYPE)
	{
		DogFoodData food = GetFood(type);
		int difference = amount - food.Amount;
		food.ChangeBy(difference);
	}

	public bool CanAfford(int cost, string type = k.DEFAULT_FOOD_TYPE)
	{
		return GetFood(type).CanAfford(cost);
	}

	DogFoodData addNewEntryToFoods(string type)
	{
		// Copy this instance so we can modify it later
		DogFoodData match = FoodDatabase.GetInstance.Get(type).Copy();
		foods[type] = match;
		return match;
	}

	#region Object Overrides 

	public override string ToString()
	{
		string foodAmounts = "{";
		foreach(DogFoodData food in foods.Values)
		{
			foodAmounts += string.Format("{0} {1}: {2}, ", food.FoodType, k.FOOD, food.Amount);
		}
		return foodAmounts.TrimEnd(new char[]{',', ' '}) + "}";
	}

	#endregion

}
