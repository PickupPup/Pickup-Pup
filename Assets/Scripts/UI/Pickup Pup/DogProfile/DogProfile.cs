/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Displays a Dog's Profile from its DogDescriptor
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogProfile : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text breedText;
    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Image dogThumbnail;
    [SerializeField]
    Image collarImage;

    [SerializeField]
    GameObject leftArrow;
    [SerializeField]
    GameObject rightArrow;

    protected DogDescriptor dogInfo;
    protected Button rehomeButton;
    protected Button collarSlot;

    public virtual void SetProfile(Dog dog)
    {
        dogInfo = dog.Info;

        nameText.text = dogInfo.Name;
        breedText.text = dogInfo.Breed.Breed;
        descriptionText.text = dogInfo.Description;

        dogThumbnail.sprite = dog.Portrait;
        // TODO: Get collar icon
    }

    public void EditName(string newName)
    {
        // TODO: Make DogDescriptor name editable
        nameText.text = dogInfo.Name;
    }
}
