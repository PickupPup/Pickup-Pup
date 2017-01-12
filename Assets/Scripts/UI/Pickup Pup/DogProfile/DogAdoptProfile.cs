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
    [SerializeField]
    Button adoptButton;

    protected override void setReferences()
    {
        base.setReferences();
        rehomeButton.interactable = false;
        collarSlot.interactable = false;
    }

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        priceText.text = dogInfo.CostToAdoptStr;
    }

    public void TryAdopt()
    {
        game.TryAdoptDog(dogInfo);
    }

}
