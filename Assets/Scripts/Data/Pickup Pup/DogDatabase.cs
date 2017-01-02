/*
 * Author: Isaiah Mann
 * Desc: Stores data about the dogs in the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class DogDatabase : Database<DogDatabase> {
	const string SPRITES_DIR = "Sprites";

	public DogBreed[] Breeds;
	public DogDescriptor[] Dogs;
	Dictionary<string, DogBreed> breedsByName;
	RandomBuffer<DogDescriptor> randomizer;
	[System.NonSerialized]
	Dictionary<DogBreed, Sprite> dogSpriteLookup = new Dictionary<DogBreed, Sprite>();

	public override void Initialize () {
		base.Initialize ();
		AssignInstance(this);
		populateDogBreedLookup();
		setDogDataReferences();
		randomizer = new RandomBuffer<DogDescriptor>(Dogs);
	}	

	public DogBreed GetBreed (string breedName) {
		DogBreed breed;
		if (breedsByName.TryGetValue(breedName, out breed)) {
			return breed;
		} else {
			return null;
		}
	}

	public DogDescriptor RandomDog () {
		return randomizer.GetRandom();
	}

	public DogDescriptor[] RandomDogList (int count) {
		return randomizer.GetRandom(count);
	}

	public Sprite GetDogBreedSprite (DogBreed breed) {
		Sprite match;
		if (dogSpriteLookup.TryGetValue(breed, out match)) {
			return match;
		} else {
			match = loadDogBreedSpriteFromSources(breed);
			if (match != null) {
				dogSpriteLookup.Add(breed, match);
				return match;
			} else {
				throw new System.Exception(string.Format("Sprite for {0} not found", breed));
			}
		}
	}
		
	void populateDogBreedLookup () {
		breedsByName = new Dictionary<string, DogBreed>();
		foreach (DogBreed breed in Breeds) {
			breed.Initialize(this);
			if (breed.Breed != null) {
				try {
					breedsByName.Add(breed.Breed, breed);
				} catch {
					Debug.LogWarningFormat("Lookup already contains breed with key {0}, Overwriting", breed.Breed);
					breedsByName[breed.Breed] = breed;
				}
			}
		}
	}

	void setDogDataReferences () {
		foreach (DogDescriptor dog in Dogs) {
			dog.Initialize(this);
		}
	}

	Sprite loadDogBreedSpriteFromSources (DogBreed breed) {
		return Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, breed.Breed));
	}
}
