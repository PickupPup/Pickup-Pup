/*
 * Author(s): Isaiah Mann 
 * Description: Stores the data about a dog
 */

using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class DogDescriptor : PPDescriptor 
{
	const float DEFAULT_TIME_TO_COLLECT = k.DEFAULT_TIME_TO_COLLECT;

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
		
	public float TotalTimeToReturn
	{
		get
		{
			return Breed.TimeToReturn;
		}
	}

	public DogBreed Breed 
	{
		get 
		{
			return database.GetBreed(breed);
		}
	}

	public string BreedName 
	{
		get 
		{
			return breed;
		}
	}

	public string Color 
	{
		get 
		{
			return this.color;
		}
	}

	public Sprite Portrait
	{
		get
		{
			return database.GetDogSprite(this);
		}
	}

	public float TimeRemainingScouting
	{
		get
		{
			return _timeRemainingScouting;
		}
	}

	public int ScoutingSlotIndex
	{
		get 
		{
			return _scoutingSlotIndex;
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

	float _timeRemainingScouting;
	int _scoutingSlotIndex;
	[System.NonSerialized]
	Dog linkedDog;

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

	public void UpdateFromSave(PPGameSave save)
	{
		_timeRemainingScouting -= save.TimeInSecSinceLastSave;
		if(_timeRemainingScouting < NONE_INT)
		{
			_timeRemainingScouting = NONE_INT;
		}
	}

	public override bool Equals(object obj)
	{
		if(obj is DogDescriptor)
		{
			DogDescriptor other = obj as DogDescriptor;
			return this.name == other.name && this.breed == other.breed && this.age == other.age;
		}
		else
		{
			return base.Equals(obj);
		}
	}

	public override int GetHashCode()
	{
		return this.name.GetHashCode() + this.breed.GetHashCode() + this.age.GetHashCode();
	}

	public void HandleScoutingBegan(int slotIndex)
	{
		if(this.IsLinkedToDog)
		{
			linkedDog.SubscribeToScoutingTimerChange(updateTimeRemainingScouting);
		}
		this._scoutingSlotIndex = slotIndex;
	}

	public void HandleScoutingEnded()
	{
		if(this.IsLinkedToDog)
		{
			linkedDog.UnsubscribeFromScoutingTimerChange(updateTimeRemainingScouting);
		}
		this._scoutingSlotIndex = NOT_FOUND_INT;
	}

	void updateTimeRemainingScouting(float timeRemainingScouting)
	{
		_timeRemainingScouting = timeRemainingScouting;
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
