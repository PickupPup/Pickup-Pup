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
	[SerializeField] DogShelterProfile dogShelterProfile;


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
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        database = DogDatabase.GetInstance;
        EventController.Event(PPEvent.LoadShelter);

        var dogs = populateAvailableDogs(database);
		dogShelterProfile.buttonController.Init (dogShelterProfile, dogs);
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
		if(gameController.TryAdoptDog(dogShelterProfile.buttonController.SelectedDogInfo))
        {
			((DogShelterSlot) selectedSlot).ShowAdopt();
            EventController.Event(k.GetPlayEvent(k.ADOPT));
			EventController.Event(k.GetPlayEvent(k.BARK), dogShelterProfile.buttonController.SelectedDogInfo.Breed.Size);
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
			dogShelterProfile.gameObject.SetActive(true);
			dogShelterProfile.SetProfile(dog);
        }     
    }

}
