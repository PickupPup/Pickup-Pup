/*
 * Author: Grace Barrett-Snyder 
 * Description: UI for displaying an individual dog's profile in the Shelter scene
 */

using UnityEngine;
using UnityEngine.UI;

public class DogAdoptProfile : DogProfile
{
    [SerializeField]
    Text priceText;

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        priceText.text = dogInfo.CostToAdoptStr;
        rehomeButton.interactable = false;
        collarSlot.interactable = false;
    }

}
