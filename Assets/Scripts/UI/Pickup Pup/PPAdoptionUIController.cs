/*
 * Author: Isaiah Mann
 * Description: Controls the adoption UI
 */

using UnityEngine;

public class PPAdoptionUIController : PPUIController
{
	DogSlot[] availableDogPortraits;
	DogDatabase data;

	protected override void SetReferences () {
		base.SetReferences ();
		availableDogPortraits = GetComponentsInChildren<DogSlot>();
	}

	protected override void FetchReferences () {
		base.FetchReferences ();
		data = DogDatabase.Instance;
		data.Initialize();
		populateAvailableDogs(data);
	}

	void populateAvailableDogs (DogDatabase data) {
		DogDescriptor[] dogs = data.RandomDogList(availableDogPortraits.Length);
		for (int i = 0; i < dogs.Length; i++) {
			availableDogPortraits[i].Init(dogs[i], data.GetDogBreedSprite(dogs[i].IBreed));
		}
	}
}
