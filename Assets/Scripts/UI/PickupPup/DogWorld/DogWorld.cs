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

    DogWorldSlot selectedDogSlot = null;
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
        deselectArea.SubscribeToClick(deselectDogSlot);
        Dog[] dogs = chooseDogs(this.dogsSlots);
        placeDogs(dogs);
    }

    #endregion

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
            Dog currentDog = dogs[i];
            DogWorldSlot currentSlot = dogsSlots[i];

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
        if(selectedDogSlot)
        {
            deselectDogSlot();
        }
        selectedDogSlot = (DogWorldSlot) dog.OccupiedSlot;
    }

    void deselectDogSlot()
    {
        selectedDogSlot.Deselect();
        selectedDogSlot = null;
    }

}
