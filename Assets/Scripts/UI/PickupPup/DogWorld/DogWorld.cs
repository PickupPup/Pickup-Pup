/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;

public class DogWorld : MonoBehaviourExtended 
{	
    [SerializeField]
    PPScene room;

    DogWorldSlot[] dogsSlots;

    #region MonoBehaviourExtended Overrides

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

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		EventController.Subscribe(handlePPDogEvent);
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		EventController.Unsubscribe(handlePPDogEvent);
	}

    #endregion

	void handlePPDogEvent(PPEvent eventType, Dog dog)
	{
		switch(eventType)
		{
			case PPEvent.DogRedeemedFromScouting:
				addDogToRoom(dog);
				break;
		}
	}

	bool addDogToRoom(Dog dog)
	{
		foreach(DogWorldSlot slot in this.dogsSlots)			
		{
			if(!slot.HasDog)
			{
				slot.Show();
				slot.Init(dog, inScoutingSelectMode:true);
				dataController.EnterRoom(dog.Info, room);
				return true;
			}
		}
		return false;
	}

    Dog[] chooseDogs(DogWorldSlot[] openSpots)
    {
        DogFactory factory = new DogFactory(hideGameObjects:true);
        Dog[] dogs = new Dog[openSpots.Length];
        DogDescriptor[] inRoom = dataController.DogsInRoom(this.room);
        DogDescriptor[] available = dataController.AvailableDogs;   
        for(int i = 0; i < openSpots.Length; i++)
        {
            int indexInAvailable = i - inRoom.Length;
            if(ArrayUtil.InRange(inRoom, i))
            {
                dogs[i] = factory.Create(inRoom[i]);
            }
            else if(ArrayUtil.InRange(available, indexInAvailable))
            {
                DogDescriptor availableDog = available[indexInAvailable];
                dogs[i] = factory.Create(availableDog);
                dataController.EnterRoom(availableDog, this.room);
            }
            else
            {
                dogs[i] = factory.Create(DogDescriptor.Default());   
            }
        }
        return dogs;
    }

    void placeDogs(Dog[] dogs)
    {
        for(int i = 0; i < this.dogsSlots.Length && i < dogs.Length; i++)
        {
            if(dogs[i].Info.EmptyDescriptor)
            {
                this.dogsSlots[i].Hide();
            }
            else
            {
                this.dogsSlots[i].Init(dogs[i], inScoutingSelectMode:true);
                if(dogs[i].IsScouting)
                {
                    this.dogsSlots[i].Hide();
                }
            }
        }
    }

}
