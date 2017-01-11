/*
 * Author: Isaiah Mann
 * Description: Controls the shelter screen
 */

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

}
