/*
 * Author: Isaiah Mann
 * Description: Controls the adoption UI
 */

using UnityEngine;
using UnityEngine.UI;

public class PPAdoptionUIController : PPUIController {

    public CurrencyDisplay coinDisplay;
    public Button adoptButton;

    Currency coins;
	DogSlot[] availableDogPortraits;
	DogDatabase data;
    HomeController homeController;

	protected override void SetReferences () {
		base.SetReferences ();
		availableDogPortraits = GetComponentsInChildren<DogSlot>();
	}

	protected override void FetchReferences () {
		base.FetchReferences ();
		data = DogDatabase.Instance;
		data.Initialize();
		populateAvailableDogs(data);
        coins = ppGameController.Coins;
        coinDisplay.SetCurrency(coins);
        homeController = HomeController.Instance;
    }

	void populateAvailableDogs (DogDatabase data) {
		DogDescriptor[] dogs = data.RandomDogList(availableDogPortraits.Length);
		for (int i = 0; i < dogs.Length; i++) {
			availableDogPortraits[i].Init(dogs[i], data.GetDogBreedSprite(dogs[i].IBreed));
		}
	}

    public void Adopt(DogDescriptor dog)
    {
        if (checkAdoption(dog))
        {
            homeController.AddDog(dog);
            // Exchange coins
        }
    }

    bool checkAdoption(DogDescriptor dog)
    {
        return (dog.CostToAdopt <= coins.Amount && homeController.AvailableSlots > 0);
    }
}
