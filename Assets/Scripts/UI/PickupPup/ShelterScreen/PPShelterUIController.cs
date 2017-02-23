/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the shelter screen
 */

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

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        promptID = PromptID.ShelterPrompt;
        base.setReferences();
        availableDogPortraits = GetComponentsInChildren<DogSlot>();
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

}
