/*
 * Author: Isaiah Mann
 * Description: Stores data about the dogs in the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class DogDatabase : Database<DogDatabase> 
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

    PPTuning tuning
    {
        get
        {
            if(dataController)
            {
                return dataController.Tuning;
            }
            else
            {
                return PPGameController.GetInstance.Tuning;
            }
        }
    }

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

	SpritesheetDatabase spriteDatabase;
    SouvenirDatabase souvenirDatabase;

    [System.NonSerialized]
    PPDataController dataController;

    public void Initialize(PPDataController dataController)
    {
        this.dataController = dataController;
        if(!IsInitialized)
        {
            this.Initialize();
        }
    }

    public override void Initialize() 
	{
		base.Initialize();
		this.spriteDatabase = SpritesheetDatabase.GetInstance;
        this.souvenirDatabase = SouvenirDatabase.GetInstance;
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

    public DogDescriptor RandomDog(bool mustBeUnadopted) 
	{
        DogDescriptor dog;
        if(dataController.AllDogsAdopted(this) && mustBeUnadopted)
        {
            dog = DogDescriptor.Default();
        }
        else
        {
            do
            {
                dog = randomizer.GetRandom();
            }
            while(mustBeUnadopted && dataController.CheckIsAdopted(dog));
        }
        return dog;
	}

    public DogDescriptor[] GetInOrderDogList(
        int count, 
        bool skipAdopted, 
        int startIndex = 0, 
        int maxMasterIndex = int.MaxValue)
    {
        DogDescriptor[] dogList = new DogDescriptor[count];
        int endIndex = startIndex + count - ZERO_INDEX_OFFSET;
        int indexInMasterDogArr = startIndex;
        for(int i = startIndex; i <= endIndex; i++)
        {
            if(ArrayUtil.InRange(this.dogs, indexInMasterDogArr))
            {
                do
                {
                    if(indexInMasterDogArr < maxMasterIndex)
                    {
                        dogList[i] = this.dogs[indexInMasterDogArr];
                    }
                    else
                    {
                        dogList[i] = DogDescriptor.Default();
                    }
                }
                while(skipAdopted &&
                    ArrayUtil.InRange(this.dogs, indexInMasterDogArr) &&
                    dataController.CheckIsAdopted(this.dogs[indexInMasterDogArr++]));
            }
            else
            {
                dogList[i] = DogDescriptor.Default();
            }
        }
        return dogList; 
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
		
    // Excludes already adopted dogs
	protected DogDescriptor[] getDailyRandomDogListFromBuffer(
		RandomBuffer<DogDescriptor> buffer, 
		int count, 
		int startIndex = 0)
	{
		buffer.Refresh();
        DogDescriptor[] fullSequence = buffer.GetRandom(dogs.Length);
		DogDescriptor[] candidates = ArrayUtil.GetRange(fullSequence, startIndex, count);
        // Allows for faster lookup versus O(n) to check array
        HashSet<DogDescriptor> currentCandidates = new HashSet<DogDescriptor>(candidates);
        // -1 for zero offset
        int currentIndex = startIndex + count - ZERO_INDEX_OFFSET;
        int totalDogCount = fullSequence.Length;
        for(int i = 0; i < candidates.Length; i++)
        {
            while(currentIndex < totalDogCount && dataController.CheckIsAdopted(candidates[i]))
            {
                if(!dataController.CheckIsAdopted(fullSequence[currentIndex]))
                {
                    if(!currentCandidates.Contains(fullSequence[currentIndex]))
                    {
                        // Remove the old dog from the Hash (because it's invalid)
                        currentCandidates.Remove(candidates[i]);
                        candidates[i] = fullSequence[currentIndex];
                        // Update the Hash w/ the new dog
                        currentCandidates.Add(candidates[i]);
                    }
                }
                currentIndex++;
            }
            currentIndex++;
            if(currentIndex >= totalDogCount && dataController.CheckIsAdopted(candidates[i]))
            {
                candidates[i] = DogDescriptor.Default();
            }
        }
        return candidates;
	}

	public DogDescriptor[] RandomDogList(int count) 
	{
		return randomizer.GetRandom(count);
	}

	public Sprite GetDogSprite(DogDescriptor dog) 
	{
		string spriteName = getSpriteName(dog);
		Sprite sprite;
		if(dog == null || !spriteDatabase.TryGetSprite(spriteName, out sprite))
		{
			return DefaultSprite;
		}
		else
		{
			return sprite;
		}
	}
	
    public Sprite GetDogWorldSprite(DogDescriptor dog)
    {
        string spriteName = getWorldSpriteName(dog);
        Sprite sprite;
        if(dog == null || !spriteDatabase.TryGetSprite(spriteName, out sprite))
        {
            return DefaultSprite;
        }
        else
        {
            return sprite;
        }
    }

    public SouvenirData GetDogSouvenir(string souvenirName)
    {
        // Always return a copy so we're not modifiying the template object:
        return souvenirDatabase.Get(souvenirName).Copy();
    }

	string getSpriteName(DogDescriptor dog)
	{
		return string.Format("{0}{1}{2}", dog.BreedName, JOIN_CHAR, dog.Color);
	}

    string getWorldSpriteName(DogDescriptor dog)
    {
        return string.Format("{0}{1}{2}{1}{3}", dog.BreedName, JOIN_CHAR, dog.Color, tuning.InWorldKey);
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

    // Bounds must be in range
    public List<DogDescriptor> GetDogRangeList(int start, int length)
    {
        return GetDogRange(start, length).ToList();
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
