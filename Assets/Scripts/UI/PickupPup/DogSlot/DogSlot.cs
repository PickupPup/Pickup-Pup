﻿/*
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

	protected Dog dog
	{
		get
		{
			return _dog;
		}
		set
		{
			// Fixes ref on previous dog
			if(_dog != null)
			{
				_dog.LeaveCurrentSlot();
			}
			// Assigns slot to new dog (assuming the new value is not null)
			if(value != null)
			{
				value.AssignSlot(this);
			}
			_dog = value;
		}
	}

	bool hasButton 
	{
		get 
		{
			return button != null;
		}
	}
		
    protected DogDescriptor dogInfo;

    protected Image[] images;

	Dog _dog;
	UIButton button;
	MonoAction onFreeSlotClick;
	PPData.DogAction onOccupiedSlotClick;

    Image backgroundImage;
    protected Image dogImage;

    bool setBackground = true;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button = ensureReference<UIButton>(searchChildren:true);
		subscribeToUIButton();
	}

	#endregion

	public int GetIndex()
	{
		return transform.GetSiblingIndex();
	}

    // Initializes this Dog Slot by setting component references and displaying its sprites.
    public virtual void Init(DogDescriptor dog, Sprite dogSprite)
    {
		this.dogInfo = dog;

		images = GetComponentsInChildren<Image>();
        dogImage = images[1];

		setSlot(this.dogInfo, dogSprite);
    }

	public virtual void ClearSlot()
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
		if(hasDogInfo && !hasDog)
		{
			this.dog = new DogFactory(hideGameObjects:true).Create(this.dogInfo);
		}
		if(hasDog)
		{
			EventController.Event(PPEvent.ClickDogSlot, this.dog);
			callOnOccupiedSlotClick(this.dog);
		}
		else 
		{
			callOnFreeSlotClick();
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

	protected bool subscribeToUIButton()
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

	protected bool unsubscribeFromUIButton()
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