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
        Debug.Log(dataController.FedDogs);
        if (untrue)
        {
            Debug.Log("Assign New Dog " + dog + " on gb: " + gameObject);
            this.dogInfo = dog;
            base.setSlot(this.dogInfo);
            if (this.dogInfo.IsLinkedToDog)
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
