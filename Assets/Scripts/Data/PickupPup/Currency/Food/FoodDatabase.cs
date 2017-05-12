/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using System.Collections.Generic;
using UnityEngine;

public class FoodDatabase : Database<FoodDatabase>
{
	[SerializeField]
	DogFoodData[] food;
	Dictionary<string, DogFoodData> dogFoodLookup;

	#region Database Overrides

	public override void Initialize()
	{
		base.Initialize();
		overwriteFromJSONInResources(FOOD, this);
		this.dogFoodLookup = generateDogFoodLookup(food);
	}

	#endregion

	Dictionary<string, DogFoodData> generateDogFoodLookup(DogFoodData[] list)
	{
		Dictionary<string, DogFoodData> lookup = new Dictionary<string, DogFoodData>();
		foreach(DogFoodData food in list)
		{
			lookup[food.FoodType] = food;
		}
		return lookup;
	}

}
