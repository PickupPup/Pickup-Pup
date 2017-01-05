/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Displays a Dog's Profile from its DogDescriptor
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogProfile : MonoBehaviourExtended
{
    [SerializeField]
    Text nameText;
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

	public virtual void SetProfile(int index, DogDescriptor dog)
    {
        this.dog = dog;
        nameText.text = dog.Name;
        descriptionText.text = "Description goes here.";

        dogThumbnail.sprite = SpriteUtil.GetDogSprite(dog.SpriteID);
        // TODO: Get collar icon

        if(index == 0) // If leftmost dog (meaning there are no dogs before this one)
        {
            leftArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
            // TODO: Check rightmost
        }
    }

    public void EditName(string newName)
    {
        dog.Name = newName;
        nameText.text = dog.Name;
    }
}
