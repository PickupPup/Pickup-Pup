/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;

public class DogWorld : MonoBehaviourExtended 
{	
	#region Instance Accessors

	public bool WanderingIsEnabled
	{
		get;
		private set;
	}

	#endregion

    [SerializeField]
    PPScene room;

    DogWorldSlot[] dogsSlots;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
		this.WanderingIsEnabled = true;
        base.setReferences();
        dogsSlots = GetComponentsInChildren<DogWorldSlot>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        Dog[] dogs = chooseDogs(this.dogsSlots);
        placeDogs(dogs);
    }

    #endregion

	public void ToggleWanderingEnabled(bool isEnabled)
	{
		this.WanderingIsEnabled = isEnabled;
		foreach(DogWorldSlot dog in this.dogsSlots)
		{
			dog.ToggleWanderingEnabled(isEnabled);
		}
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
