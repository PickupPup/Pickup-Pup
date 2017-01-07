/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Parent class for a Dog's thumbnail.
 */

using UnityEngine;
using UnityEngine.UI;

public class DogSlot : MonoBehaviourExtended
{
    protected PPDataController dataController;
    protected DogDescriptor dog;
    protected Image[] images;

    Image backgroundImage;
    Image dogImage;

    bool setBackground = true;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = (PPDataController) PPDataController.Instance;
    }

    #endregion

    // Initializes this Dog Slot by setting component references and displaying its sprites.
    public virtual void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
		this.dog = dog;

		images = GetComponentsInChildren<Image>();
		if(images.Length >= 2) 
		{
            if (setBackground)
            {
                backgroundImage = images[0];
                dogImage = images[1];
            }
	        else
            {
                dogImage = images[0];
            }
		}

		setSlot(this.dog, dogSprite, backgroundSprite);
    }
	
    // Sets the dog and background sprites of this Dog Slot.
	void setSlot(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        dogImage.sprite = dogSprite;
		if(backgroundImage)
        {
        	backgroundImage.sprite = backgroundSprite;
		}
    }

}
