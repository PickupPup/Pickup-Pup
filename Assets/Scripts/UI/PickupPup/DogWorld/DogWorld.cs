/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class DogWorld : PPUIController 
{	
    DogWorldSlot[] dogsSlots;

    protected override void setReferences()
    {
        base.setReferences();
        dogsSlots = GetComponentsInChildren<DogWorldSlot>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        Dog[] dogs = chooseDogs(this.dogsSlots);
        placeDogs(dogs);
    }

    Dog[] chooseDogs(DogWorldSlot[] openSpots)
    {
        DogFactory factory = new DogFactory(hideGameObjects:true);
        Dog[] dogs = new Dog[openSpots.Length];
        DogDescriptor[] available = dataController.AvailableDogs;   
        for(int i = 0; i < openSpots.Length; i++)
        {
            dogs[i] = factory.Create(available[i]);
        }
        return dogs;
    }

    void placeDogs(Dog[] dogs)
    {
        for(int i = 0; i < this.dogsSlots.Length && i < dogs.Length; i++)
        {
            this.dogsSlots[i].Init(dogs[i], inScoutingSelectMode:false);
        }
    }

}
