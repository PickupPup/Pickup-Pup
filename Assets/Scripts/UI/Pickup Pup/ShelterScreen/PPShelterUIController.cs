/*
 * Author: Isaiah Mann
 * Description: Controls the shelter screen
 */

public class PPShelterUIController : PPUIController
{
    DogSlot[] availableDogPortraits;
    DogDatabase database;
    DogAdoptProfile dogProfile;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        availableDogPortraits = GetComponentsInChildren<DogSlot>();
        dogProfile = GetComponentInChildren<DogAdoptProfile>();
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
        for (int i = 0; i < dogs.Length; i++)
        {
            DogDescriptor dog = dogs[i];
            availableDogPortraits[i].Init(dog, database.GetDogBreedSprite(dog.Breed));
        }
    }

    void showDogProfile()
    {

    }

}
