/*
 * Author(s): Isaiah Mann 
 * Description: Stores the data about a dog
 */

using UnityEngine;

[System.Serializable]
public class DogDescriptor : PPDescriptor 
{
	const float DEFAULT_TIME_TO_COLLECT = 10f;

	#region Instance Accessors

	public bool IsLinkedToDog 
	{
		get 
		{
			return linkedDog != null;
		}
	}
		
	public float RemainingTimeScouting
	{
		get 
		{
			if(IsLinkedToDog) 
			{
				return linkedDog.RemainingTimeScouting;
			}
			else 
			{
				return default(float);
			}
		}
	}

	public int CostToAdopt 
	{
		get 
		{
			if(hasSpecialCost) 
			{
				return ModCost;
			} 
			else 
			{
				return IBreed.CostToAdopt;
			}
		}
	}
	public string CostToAdoptStr 
	{
		get 
		{
			if(hasSpecialCost) 
			{
				return formatCost(ModCost);
			} 
			else 
			{
				return IBreed.CostToAdoptStr;
			}
		}
	}

	public DogBreed IBreed 
	{
		get 
		{
			return data.GetBreed(Breed);
		}
	}
	public Color IColor 
	{
		get 
		{
			return parseHexColor(Color);
		}
	}

	#endregion


	bool hasSpecialCost 
	{
		get 
		{
			return ModCost != 0;
		}
	}

	public string Name;
	public string Color;
	public string Breed;
	public int ModCost;
	public int Age;
	public int SpriteID;
	public bool IsOutCollecting = false;

	Dog linkedDog;
	DogBreed _iBreed;


	public static DogDescriptor Default() 
	{
		DogDescriptor descriptor = new DogDescriptor(DogDatabase.Instance);
		descriptor.Name = string.Empty;
		descriptor.Age = 0;
		descriptor.Breed = string.Empty;
		descriptor.Color = BLACK_HEX;
		descriptor.SpriteID = 0;
		descriptor.IsOutCollecting = false;
		return descriptor;
	}
	public DogDescriptor(DogDatabase data) : base(data) 
	{
		// NOTHING
	}

	public void LinkToDog(Dog dog) 
	{
		this.linkedDog = dog;
	}

	public void UnlinkFromDog() 
	{
		this.linkedDog = null;
	}

}
