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
    UIButton closeWindowHitArea;

    [SerializeField]
    protected Button rehomeButton;
    [SerializeField]
    protected Button collarSlot;

    protected DogDescriptor dogInfo;

    #region MonoBehaviourExtended Overrides

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
        for(int i = 0; i < dogInfo.Descriptions.Length; i++)
        {
            descriptionText[i].text = dogInfo.Descriptions[i];
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
