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
	static Sprite defaultSprite
	{
		get 
		{
			if(_defaultSprite)
			{
				return _defaultSprite;
			} 
			else 
			{
				// Memoization for effeciency :
				_defaultSprite = Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, DEFAULT));
				return _defaultSprite;
			}
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

	RandomBuffer<DogDescriptor> randomizer;
	Dictionary<string, DogBreed> breedsByName;
	[System.NonSerialized]
	Dictionary<DogBreed, Sprite> dogSpriteLookup = new Dictionary<DogBreed, Sprite>();

	public override void Initialize() 
	{
		AssignInstance(this);
		populateDogBreedLookup();
		setDogDataReferences();
		randomizer = new RandomBuffer<DogDescriptor>(dogs);
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

	public DogDescriptor[] RandomDogList(int count) 
	{
		return randomizer.GetRandom(count);
	}

	public Sprite GetDogBreedSprite(DogBreed breed) 
	{
		// Error checking
		if(breed == null || string.IsNullOrEmpty(breed.Breed))
		{
			return defaultSprite;
		}

		Sprite match;
		if(dogSpriteLookup.TryGetValue(breed, out match)) 
		{
			return match;
		} 
		else 
		{
			match = loadDogBreedSpriteFromSources(breed);
			if(match != null) 
			{
				dogSpriteLookup.Add(breed, match);
				return match;
			}
			else
			{
				throw new System.Exception(string.Format("Sprite for {0} not found", breed));
			}
		}
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

	Sprite loadDogBreedSpriteFromSources(DogBreed breed) 
	{
		return Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, breed.Breed));
	}

}
