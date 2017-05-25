/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using System.Collections.Generic;
using UnityEngine;
using k = PPGlobal;

public class FoodDatabase : Database<FoodDatabase>
{
	#region Static Accessors

	public static Sprite DefaultSprite
	{
		get 
		{
			if(_defaultSprite)
			{
				return _defaultSprite;
			} 
			else 
			{
				// Memoization for efficiency
				if(SpritesheetDatabase.GetInstance.TryGetSprite(k.DEFAULT_FOOD_SPRITE, out _defaultSprite))
				{
					return _defaultSprite;
				}
				else
				{
					throw new SpriteNotFoundException();
				}
			}
		}
	}

	#endregion

	static Sprite _defaultSprite;

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

	public DogFoodData Get(string foodType)
	{
		DogFoodData food;
		if(!dogFoodLookup.TryGetValue(foodType, out food))
		{
			food = DogFoodData.Default();
		}
		return food;
	}

	public Sprite GetSprite()
	{
		return DefaultSprite;
	}

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
