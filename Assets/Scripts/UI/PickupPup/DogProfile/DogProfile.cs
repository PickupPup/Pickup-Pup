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
    Text[] descriptionText; 

    [SerializeField]
    Image dogThumbnail;
    [SerializeField]
    Image collarImage;

    [SerializeField]
    GameObject leftArrow;
    [SerializeField]
    GameObject rightArrow;
    [SerializeField]
    UIElement[] descriptionFields; // Normal description must come first (not special)

    [SerializeField]
    UIButton closeWindowHitArea;

    [SerializeField]
    protected GameObject iconsObject;
    [SerializeField]
    protected Button rehomeButton;
    [SerializeField]
    protected Button collarSlot;

    protected DogDescriptor dogInfo;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();

        descriptionText = new Text[descriptionFields.Length];
        for(int i = 0; i < descriptionText.Length; i++)
        {
            descriptionText[i] = descriptionFields[i].GetComponentInChildren<Text>();
        }
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        closeWindowHitArea.SubscribeToClick(Hide);
    }

    #endregion

    public virtual void SetProfile(Dog dog)
    {
        dogInfo = dog.Info;

        nameText.text = dogInfo.Name;
        breedText.text = dogInfo.Breed.Breed;
        for(int i = 0; i < descriptionText.Length; i++)
        {
            if (i < dogInfo.Descriptions.Length)
            {
                descriptionFields[i].Show();
                descriptionText[i].text = dogInfo.Descriptions[i];
            }
            else
            {
                descriptionFields[i].Hide();
            }
        }

        dogThumbnail.sprite = dog.Portrait;
        // TODO: Get collar icon
    }

    public void EditName(string newName)
    {
        // TODO: Make DogDescriptor name editable
        nameText.text = dogInfo.Name;
    }
}
