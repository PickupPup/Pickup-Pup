/*
 * Author: Isaiah Mann
 * Description: Stores data about the dogs in the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class DogDatabase : Database<DogDatabase> 
{
	#region Static Accessors

	public static DogDatabase GetInstance
	{
		get
		{
			DogDatabase database = Instance;
			// Initializes the database if it's not already setup
			database.TryInit();
			return database;
		}
	}

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
				_defaultSprite = Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, DEFAULT));
				return _defaultSprite;
			}
		}
	}

	#endregion

	static string defaultJSONData
	{
		get
		{
			TextAsset json = Resources.Load<TextAsset>(Path.Combine(JSON_DIR, GAME_DATA));
			return json.text;
		}
	}

	static Sprite _defaultSprite;

	#region Instance Accessors

	public DogDescriptor[] Dogs
	{
		get
		{
			return this.dogs;
		}
	}

	#endregion

	[SerializeField]
	DogBreed[] breeds;
	[SerializeField]
	DogDescriptor[] dogs;

	[System.NonSerialized]
	RandomBuffer<DogDescriptor> randomizer;
	// This buffer is used to generate same sequence of dogs based off day
	[System.NonSerialized]
	RandomBuffer<DogDescriptor> dailyRandomizer;

	Dictionary<string, DogBreed> breedsByName;
	[System.NonSerialized]
	Dictionary<DogDescriptor, Sprite> dogSpriteLookup = new Dictionary<DogDescriptor, Sprite>();

	public override void Initialize() 
	{
		base.Initialize();
		AssignInstance(this);
		populateDogBreedLookup();
		setDogDataReferences();
		randomizer = new RandomBuffer<DogDescriptor>(dogs);
        dailyRandomizer = new RandomDailyBuffer<DogDescriptor>(dogs);
    }	

	public DogBreed GetBreed(string breedName) 
	{
		// Error checking
		if(string.IsNullOrEmpty(breedName))
		{
			return DogBreed.Default;
		}

		DogBreed breed;
		if(breedsByName.TryGetValue(breedName, out breed)) 
		{
			return breed;
		}
		else 
		{
			return null;
		}
	}

	public DogDescriptor RandomDog() 
	{
		return randomizer.GetRandom();
	}

	// Returns sequence based on day
	// Always starts from beginning unless start index is different
	public DogDescriptor[] GetDailyRandomDogList(int count, int startIndex = 0)
	{
		return getDailyRandomDogListFromBuffer(dailyRandomizer, count, startIndex);
	}

	// Override to simulate a different day than current:
	public DogDescriptor[] GetDailyRandomDogList(System.DateTime day, int count, int startIndex = 0)
	{
		return getDailyRandomDogListFromBuffer(
			new RandomDailyBuffer<DogDescriptor>(dogs, day), count, startIndex);
	}
		
	protected DogDescriptor[] getDailyRandomDogListFromBuffer(
		RandomBuffer<DogDescriptor> buffer, 
		int count, 
		int startIndex = 0)
	{
		buffer.Refresh();
		int length = startIndex + count;
		DogDescriptor[] fullSequence = buffer.GetRandom(length);
		return ArrayUtil.GetRange(fullSequence, startIndex, count);
	}

	public DogDescriptor[] RandomDogList(int count) 
	{
		return randomizer.GetRandom(count);
	}

	public Sprite GetDogSprite(DogDescriptor dog) 
	{
		checkSpriteLookup();
		// Error checking
		if(dog == null)
		{
			return DefaultSprite;
		}

		Sprite match;
		if(dogSpriteLookup.TryGetValue(dog, out match)) 
		{
			return match;
		} 
		else 
		{
			match = loadSpriteFromResources(dog);
			if(match != null) 
			{
				dogSpriteLookup.Add(dog, match);
				return match;
			}
			else
			{
				return DefaultSprite;
			}
		}
	}

	void checkSpriteLookup()
	{
		if(dogSpriteLookup == null)
		{
			dogSpriteLookup = new Dictionary<DogDescriptor, Sprite>();
		}
	}

	public override bool TryInit()
	{
		if(tryInitData())
		{
			return base.TryInit();
		}
		else
		{
			return false;
		}
	}

	// Bounds must be in range
	public DogDescriptor[] GetDogRange(int start, int length)
	{
		if(inRangeOfDogsArr(start, length))
		{
			return ArrayUtil.GetRange(this.dogs, start, length);	
		}
		else
		{
			return new DogDescriptor[0];
		}
	}

	bool inRangeOfDogsArr(int start, int length)
	{
		return ArrayUtil.InRange(this.dogs, start, length);	
	}

	void populateDogBreedLookup() 
	{
		breedsByName = new Dictionary<string, DogBreed>();
		foreach(DogBreed breed in breeds)
		{
			breed.Initialize(this);
			if(breed.Breed != null) 
			{
				try
				{
					breedsByName.Add(breed.Breed, breed);
				}
				catch
				{
					Debug.LogWarningFormat("Lookup already contains breed with key {0}, Overwriting", breed.Breed);
					breedsByName[breed.Breed] = breed;
				}
			}
		}
	}

	void setDogDataReferences() 
	{
		foreach(DogDescriptor dog in dogs) 
		{
			dog.Initialize(this);
		}
	}

	Sprite loadSpriteFromResources(DogDescriptor dog) 
	{
//		string fileName = string.Format("{0}{1}{2}", dog.BreedName, dog.a
//		string path = Path.Combine(SPRITES_DIR, fileName);
		//return Resources.Load<Sprite>(path);
		throw new System.NotImplementedException();
	}

	// Returns false if data is already initialized
	bool tryInitData()
	{
		if(dataIsInitialized())
		{
			return false;
		}
		else
		{
			JsonUtility.FromJsonOverwrite(defaultJSONData, this);
			return true;
		}
	}

	bool dataIsInitialized()
	{
		return this.dogs != null && this.breeds != null;
	}

}
