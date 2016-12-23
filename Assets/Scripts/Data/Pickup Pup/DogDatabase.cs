using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogDatabase : Database<DogDatabase> {
	public DogBreed[] Breeds;
	public DogDescriptor[] Dogs;
	Dictionary<string, DogBreed> breedsByName;

	public override void Initialize () {
		base.Initialize ();
		AssignInstance(this);
		populateDogBreedLookup();
		setDogDataReferences();
	}	

	public DogBreed GetBreed (string breedName) {
		DogBreed breed;
		if (breedsByName.TryGetValue(breedName, out breed)) {
			return breed;
		} else {
			return null;
		}
	}

	void populateDogBreedLookup () {
		breedsByName = new Dictionary<string, DogBreed>();
		foreach (DogBreed breed in Breeds) {
			breed.Initialize(this);
			if (breed.Name != null) {
				try {
					breedsByName.Add(breed.Name, breed);
				} catch {
					Debug.LogWarningFormat("Lookup already contains breed with key {0}, Overwriting", breed.Name);
					breedsByName[breed.Name] = breed;
				}
			}
		}
	}

	void setDogDataReferences () {
		foreach (DogDescriptor dog in Dogs) {
			dog.Initialize(this);
		}
	}

}
