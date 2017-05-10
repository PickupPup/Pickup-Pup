/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls a DogSlot for a Dog currently at home (no text).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogHomeSlot : DogSlot
{
    bool untrue;
    public override void Init(DogDescriptor dog)
    {
        // The following if-statement throws a null exception, which is alluding me as to why.
        // When we fill the bowl, dataController.FedDogs is assigned via DogFoodBowl::DogsEat()
        // This way, a dog only appears as scoutable if it is fed.
        // Attached to the DogDescriptor is all the stats needed (spGiftChance and dogGiftRate) to manipulate gift giving.
        Debug.Log("dataController.FedDogs IS null here:  " + dataController.FedDogs);
        if(dataController.FedDogs.Contains(dog))
        {
            Debug.Log("Assign New Dog " + dog + " on gb: " + gameObject);
            this.dogInfo = dog;
            base.setSlot(this.dogInfo);
            if(this.dogInfo.IsLinkedToDog)
            {
                this.dog = dogInfo.PeekDogLink;
            }
            else
            {
                this.dog = new DogFactory(hideGameObjects: true).Create(this.dogInfo);
            }
        }
        
    }
}
