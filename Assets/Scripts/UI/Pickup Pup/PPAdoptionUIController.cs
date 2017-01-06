/*
 * Author: Isaiah Mann
 * Description: Controls the adoption UI
 */

using UnityEngine;

public class PPAdoptionUIController : PPUIController 
{	
	DogSlot[] availableDogPortraits;
	DogDatabase database;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		availableDogPortraits = GetComponentsInChildren<DogSlot>();
	}

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		database = DogDatabase.Instance;
		database.Initialize();
		populateAvailableDogs(database);
	}

	#endregion

	void populateAvailableDogs(DogDatabase data) 
	{
		DogDescriptor[] dogs = data.RandomDogList(availableDogPortraits.Length);
		for(int i = 0; i < dogs.Length; i++) 
		{
			availableDogPortraits[i].Init(dogs[i], data.GetDogBreedSprite(dogs[i].Breed));
		}
	}

}
