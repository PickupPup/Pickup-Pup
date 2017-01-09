/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Parent class for a Dog's thumbnail.
 */

using UnityEngine;
using UnityEngine.UI;

public class DogSlot : PPUIElement
{
	protected bool hasDogInfo
	{
		get
		{
			return dogInfo != null;
		}
	}

	protected bool hasDog
	{
		get
		{
			return dog != null;
		}
	}

    protected PPGameController game;
    protected DogDescriptor dogInfo;
	protected Dog dog;
    protected Image[] images;

	PPData.DogAction onSlotClick;

    Image backgroundImage;
    Image dogImage;

    bool setBackground = true;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        game = PPGameController.GetInstance;
    }

    #endregion

    // Initializes this Dog Slot by setting component references and displaying its sprites.
    public virtual void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
		this.dogInfo = dog;

		images = GetComponentsInChildren<Image>();
		if(images.Length >= 2) 
		{
            if(setBackground)
            {
                backgroundImage = images[0];
                dogImage = images[1];
            }
	        else
            {
                dogImage = images[0];
            }
		}

		setSlot(this.dogInfo, dogSprite, backgroundSprite);
    }

	public void ClearSlot()
	{
		this.dog = null;
		this.dogInfo = null;
	}

	public virtual void Init(Dog dog)
	{
		this.dog = dog;
		Init(dog.Info, dog.Portrait);
	}

	public void ExecuteClick()
	{
		if(hasDog)
		{
			callOnSlotClick(this.dog);
		}
	}

	public void SubscribeToClick(PPData.DogAction clickAction)
	{
		onSlotClick += clickAction;
	}

	public void UnsubscribeFromClick(PPData.DogAction clickAction)
	{
		onSlotClick -= clickAction;
	}

	void callOnSlotClick(Dog dog)
	{
		if(onSlotClick != null)
		{
			onSlotClick(dog);
		}
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
