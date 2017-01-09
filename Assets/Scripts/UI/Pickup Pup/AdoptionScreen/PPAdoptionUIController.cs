/*
 * Author: Isaiah Mann
 * Description: Controls the adoption UI
 */

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
		database = DogDatabase.GetInstance;
		populateAvailableDogs(database);
	}

	#endregion

	void populateAvailableDogs(DogDatabase database) 
	{
		DogDescriptor[] dogs = database.RandomDogList(availableDogPortraits.Length);
		for(int i = 0; i < dogs.Length; i++) 
		{
			availableDogPortraits[i].Init(dogs[i], database.GetDogBreedSprite(dogs[i].Breed));
		}
	}

}
