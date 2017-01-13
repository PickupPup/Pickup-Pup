﻿/*
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
		
	public string Name 
	{
		get 
		{
			return name;
		}
	}

	public string CostToAdoptStr 
	{
		get 
		{
			if(hasSpecialCost) 
			{
				return formatCost(modCost);
			} 
			else 
			{
				return Breed.CostToAdoptStr;
			}
		}
	}

    public string[] Descriptions
    {
        get
        {
            return description;
        }
    }

	public int Age 
	{
		get 
		{
			return age;
		}
	}

	public int CostToAdopt 
	{
		get 
		{
			if(hasSpecialCost) 
			{
				return modCost;
			} 
			else 
			{
				return Breed.CostToAdopt;
			}
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

	public DogBreed Breed 
	{
		get 
		{
			return database.GetBreed(breed);
		}
	}

	public Color Color 
	{
		get 
		{
			return parseHexColor(this.color);
		}
	}

	public Sprite Portrait
	{
		get
		{
			return database.GetDogBreedSprite(this.Breed);
		}
	}

	#endregion

	bool hasSpecialCost 
	{
		get 
		{
			return modCost != 0;
		}
	}

	[SerializeField]
	string name;
	[SerializeField]
	string color;
	[SerializeField]
	string breed;
	[SerializeField]
	int modCost;
	[SerializeField]
	int age;
    [SerializeField]
    string[] description;

	[System.NonSerialized]
	Dog linkedDog;
	DogBreed _iBreed;

	public static DogDescriptor Default() 
	{
		DogDescriptor descriptor = new DogDescriptor(DogDatabase.GetInstance);
		descriptor.name = string.Empty;
		descriptor.age = 0;
		descriptor.breed = string.Empty;
		descriptor.color = BLACK_HEX;
        descriptor.description = new string[] 
            {
                string.Empty, string.Empty
            };
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
