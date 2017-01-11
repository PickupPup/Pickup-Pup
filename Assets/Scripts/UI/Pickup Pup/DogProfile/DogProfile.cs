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

    protected DogDescriptor dog;

    protected override void fetchReferences()
    {
        base.fetchReferences();
        SetProfile(1, game.Data.Dogs[1]);
    }

    public virtual void SetProfile(int index, DogDescriptor dog)
    {
        this.dog = dog;

        nameText.text = dog.Name;
        breedText.text = dog.Breed.Breed;
        descriptionText.text = dog.Description;

        dogThumbnail.sprite = dog.Portrait;
        // TODO: Get collar icon
    }

    public void EditName(string newName)
    {
        //dog.Name = newName;
        nameText.text = dog.Name;
    }
}
