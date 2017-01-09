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

	bool hasButton {
		get {
			return button != null;
		}
	}
		
    protected DogDescriptor dogInfo;
	protected Dog dog;
    protected Image[] images;

	UIButton button;
	MonoAction onFreeSlotClick;
	PPData.DogAction onOccupiedSlotClick;

    Image backgroundImage;
    Image dogImage;

    bool setBackground = true;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button = GetComponentInChildren<UIButton>();
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
			callOnOccupiedSlotClick(this.dog);
		}
		else 
		{
			callOnFreeSlotClick();
		}
	}

	public bool SubscribeToUIButton()
	{
		if(hasButton)	
		{
			button.SubscribeToClick(ExecuteClick);
			return true;
		} 
		else 
		{
			return false;
		}
	}

	public bool UnsubscribeFromUIButton()
	{
		if(hasButton)
		{
			button.UnsubscribeFromClick(ExecuteClick);
			return true;
		}
		else 
		{
			return false;
		}
	}

	public void SubscribeToClickWhenOccupied(PPData.DogAction clickAction)
	{
		onOccupiedSlotClick += clickAction;
	}

	public void UnsubscribeFromClickWhenOccupied(PPData.DogAction clickAction)
	{
		onOccupiedSlotClick -= clickAction;
	}

	public void SubscribeToClickWhenFree(MonoAction clickAction)
	{
		onFreeSlotClick += clickAction;
	}

	public void UnsubscribeFromClickWhenFree(MonoAction clickAction)
	{
		onFreeSlotClick -= clickAction;
	}

	void callOnOccupiedSlotClick(Dog dog)
	{
		if(onOccupiedSlotClick != null)
		{
			onOccupiedSlotClick(dog);
		}
	}

	void callOnFreeSlotClick()
	{
		if(onFreeSlotClick != null)
		{
			onFreeSlotClick();
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
