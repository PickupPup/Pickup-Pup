/*
 * Author: Isaiah Mann
 * Description: Controls the adoption UI
 */

using UnityEngine;
using UnityEngine.UI;

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

<<<<<<< HEAD
	protected override void FetchReferences () {
		base.FetchReferences ();
		data = DogDatabase.Instance;
		data.Initialize();
		populateAvailableDogs(data);
        coins = ppGameController.Coins;
        coinDisplay.SetCurrency(coins);
        homeController = HomeController.Instance;
    }
=======
	protected override void fetchReferences() 
	{
		base.fetchReferences();
		database = DogDatabase.Instance;
		database.Initialize();
		populateAvailableDogs(database);
	}
>>>>>>> origin/master

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
