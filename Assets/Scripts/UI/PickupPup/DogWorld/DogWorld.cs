/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls placement and selection of dogs in the world.
 * Usage: [no notes]
 */

using UnityEngine;

public class DogWorld : MonoBehaviourExtended 
{	
    [SerializeField]
    PPScene room;
    [SerializeField]
    UIButton deselectArea;

    DogWorldSlot[] dogSlots;
    DogWorldSlot selectedDogSlot = null;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        deselectArea.SubscribeToClick(deselectDogSlot);
        dogSlots = GetComponentsInChildren<DogWorldSlot>();
        Dog[] dogs = chooseDogs(dogSlots.Length);
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
		foreach(DogWorldSlot slot in this.dogSlots)			
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

    Dog[] chooseDogs(int numOfOpenSpots)
    {
        DogFactory factory = new DogFactory(hideGameObjects:true);
        Dog[] dogs = new Dog[numOfOpenSpots];
        DogDescriptor[] inRoom = dataController.DogsInRoom(this.room);
        DogDescriptor[] available = dataController.AvailableDogs;   
        for(int i = 0; i < numOfOpenSpots; i++)
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
        for(int i = 0; i < dogSlots.Length && i < dogs.Length; i++)
        {
            Dog currentDog = dogs[i];
            DogSlot currentSlot = dogSlots[i];

            if(currentDog.Info.EmptyDescriptor)
            {
                currentSlot.Hide();
            }
            else
            {
                currentSlot.Init(currentDog, inScoutingSelectMode:true);
                currentSlot.SubscribeToClickWhenOccupied(selectDogSlot);
                if(currentDog.IsScouting)
                {
                    currentSlot.Hide();
                }
            }
        }
    }

    void selectDogSlot(Dog dog)
    {
        if(selectedDogSlot && !selectedDogSlot.Equals(dog.OccupiedSlot))
        {
            deselectDogSlot();
        }
        selectedDogSlot = (DogWorldSlot) dog.OccupiedSlot;
    }

    void deselectDogSlot()
    {
		if(selectedDogSlot)
		{
			selectedDogSlot.Deselect();
			selectedDogSlot = null;
		}
    }

}
