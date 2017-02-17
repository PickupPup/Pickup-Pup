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

    [SerializeField]
    GameObject dogShelterProfileObject;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        promptID = PromptID.ShelterPrompt;
        base.setReferences();
        availableDogPortraits = GetComponentsInChildren<DogSlot>();
        if (dogShelterProfileObject != null)
        {
            dogShelterProfileObject.SetActive(false);
        }
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        database = DogDatabase.GetInstance;
        EventController.Event(PPEvent.LoadShelter);
        populateAvailableDogs(database);
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

    void populateAvailableDogs(DogDatabase database)
    {
        DogDescriptor[] dogs = database.GetDailyRandomDogList(availableDogPortraits.Length);
        for(int i = 0; i < dogs.Length; i++)
        {
            DogDescriptor dog = dogs[i];
			availableDogPortraits[i].Init(dog, database.GetDogSprite(dog));
        }
    }

	// Need a void version in order to use w/ the Unity Event System (on a button click)
	public void Adopt()
	{
		TryAdopt();
	}

    public bool TryAdopt()
    {
        DogDescriptor selectedDogInfo = selectedDog.Info;
        if(gameController.TryAdoptDog(selectedDogInfo))
        {
            ((DogShelterSlot) selectedDog.OccupiedSlot).ShowAdopt();
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
            dogShelterProfileObject.SetActive(true);
            if (!dogShelterProfile)
            {
                dogShelterProfile = dogShelterProfileObject.GetComponent<DogShelterProfile>();
            }
            dogShelterProfile.SetProfile(dog);
        }     
    }

}
