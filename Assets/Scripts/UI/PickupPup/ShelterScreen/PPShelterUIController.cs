/*
 * Author: Isaiah Mann
 * Description: Controls the shelter screen
 */

using k = PPGlobal;

public class PPShelterUIController : PPUIController
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
        EventController.Event(PPEvent.LoadShelter);
        populateAvailableDogs(database);
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
            ((DogAdoptionSlot) selectedDog.OccupiedSlot).ShowAdopt();
            EventController.Event(k.GetPlayEvent(k.ADOPT));
            EventController.Event(k.GetPlayEvent(k.BARK), selectedDogInfo.Breed.Size);
            return true;
        }
        return false;
    }

}
