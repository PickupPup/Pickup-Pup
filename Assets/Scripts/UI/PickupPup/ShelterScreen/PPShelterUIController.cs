/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the shelter screen
 */

using UnityEngine;
using k = PPGlobal;

public class PPShelterUIController : PPUIController
{
    PPTuning tuning 
    {
        get
        {
            if(gameController) 
            {
                return gameController.Tuning;
            }
            else
            {
                return new PPTuning();
            }
        }
    }

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
        DogDescriptor[] dogs;
        if(tuning.SampleShelterDogsInOrder)
        {
            dogs = database.GetInOrderDogList(
                availableDogPortraits.Length, 
                skipAdopted:true, 
                startIndex:0, 
                maxMasterIndex:(tuning.ShouldLimitShelterDogs ? 
                    tuning.ShelterDogsLimit : int.MaxValue));
        }
        else
        {
            dogs = database.GetDailyRandomDogList(availableDogPortraits.Length);
        }
        dogs = fillInEmptySlots(dogs);
        for(int i = 0; i < dogs.Length; i++)
        {
            availableDogPortraits[i].Init(dogs[i]);
        }
		return dogs;
    }

    // Fills in empty slots w/ already adopted dogs
    DogDescriptor[] fillInEmptySlots(DogDescriptor[] listWithEmpties)
    {
        DogDescriptor[] revisedList = listWithEmpties;
        DogDescriptor[] adoptedList = dataController.AdoptedDogs.ToArray();
        int indexInAdopted = adoptedList.Length - SINGLE_VALUE;
        for(int i = 0; i < listWithEmpties.Length; i++)
        {
            if(listWithEmpties[i].EmptyDescriptor && 
                ArrayUtil.InRange(adoptedList, indexInAdopted))
            {
                revisedList[i] = adoptedList[indexInAdopted--];
            }
        }
		return revisedList;
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
			DogShelterSlot slot = (DogShelterSlot)availableDogPortraits [dogShelterProfile.buttonController.SelectedIndex];
			slot.ShowAdopt();
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
