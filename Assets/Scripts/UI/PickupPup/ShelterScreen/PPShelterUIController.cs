/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the shelter screen
 */

using UnityEngine;
using k = PPGlobal;

public class PPShelterUIController : PPUIController
{
    DogSlot[] availableDogPortraits;
    DogDatabase database;
    DogShelterProfile dogShelterProfile;
	DogProfileButtonController dogProfileButtonController;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        promptID = PromptID.ShelterPrompt;
        base.setReferences();
        availableDogPortraits = GetComponentsInChildren<DogSlot>();
		if (dogProfileObject != null)
        {
			dogProfileObject.SetActive(false);
        }
		if (!dogShelterProfile)
		{
			dogShelterProfile = dogProfileObject.GetComponent<DogShelterProfile>();
		}
		if (!dogProfileButtonController)
		{
			dogProfileButtonController = dogProfileObject.GetComponent<DogProfileButtonController>();
		}
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        database = DogDatabase.GetInstance;
        EventController.Event(PPEvent.LoadShelter);
        var dogs = populateAvailableDogs(database);
		dogProfileButtonController.Init (dogShelterProfile, dogs);
		dogProfileButtonController.OnSwitchProfile += (index) => {
			selectedDogInfo = availableDogPortraits[index].PeekDogInfo;
			selectedSlot = availableDogPortraits[index];
		};
    }

    #endregion

    #region PPUIController Overrides

    protected override void showPopupPrompt()
    {
        if(!PlayerPrefsUtil.ShowedShelterPrompt)
        {
            base.showPopupPrompt();
            PlayerPrefsUtil.ShowedShelterPrompt = true;
        }  
    }

	protected override void handleDogSlotClicked (Dog dog)
	{
		base.handleDogSlotClicked (dog);
		for (int i = 0; i < availableDogPortraits.Length; i++) {
			if (availableDogPortraits [i] == dog.OccupiedSlot) {
				dogProfileButtonController.SetCurrentIndex (i);
			}
		}
	}

    #endregion

	DogDescriptor[] populateAvailableDogs(DogDatabase database)
    {
        DogDescriptor[] dogs = database.GetDailyRandomDogList(availableDogPortraits.Length);
        for(int i = 0; i < dogs.Length; i++)
        {
            DogDescriptor dog = dogs[i];
			availableDogPortraits[i].Init(dog, database.GetDogSprite(dog));
        }
		return dogs;
    }

	// Need a void version in order to use w/ the Unity Event System (on a button click)
	public void Adopt()
	{
		TryAdopt();
	}

    public bool TryAdopt()
    {
        if(gameController.TryAdoptDog(selectedDogInfo))
        {
			((DogShelterSlot) selectedSlot).ShowAdopt();
            EventController.Event(k.GetPlayEvent(k.ADOPT));
            EventController.Event(k.GetPlayEvent(k.BARK), selectedDogInfo.Breed.Size);
            return true;
        }
        return false;
    }

    protected override void showDogProfile(Dog dog)
    {      
        if(dataController.AdoptedDogs.Contains(dog.Info))
        {
            base.showDogProfile(dog);
        }
        else
        {
            EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
			if (dogProfileObject != null)
			{
				dogProfileObject.SetActive(true);
			}
            dogShelterProfile.SetProfile(dog);
        }     
    }

}
