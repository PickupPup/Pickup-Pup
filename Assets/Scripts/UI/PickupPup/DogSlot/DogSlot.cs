/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Parent class for a Dog's thumbnail.
 */

using UnityEngine;
using UnityEngine.UI;

public class DogSlot : PPUIElement
{
    #region Instance Accessors 

    public Dog PeekDog
    {
        get
        {
            return this.dog;
        }
    }

    public bool HasDog
    {
        get
        {
            return dog != null;
        }
    }

    #endregion

	protected bool hasDogInfo
	{
		get
		{
			return dogInfo != null;
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

	protected virtual bool showProfileOnClick
	{
		get
		{
			return !inScoutingSelectMode;
		}
	}

    protected DogDescriptor dogInfo;

    protected Image[] images;

	Dog _dog;
	UIButton button;
	MonoAction onFreeSlotClick;
	PPData.DogAction onOccupiedSlotClick;

    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    protected Image dogImage;

    bool setBackground = true;
	bool inScoutingSelectMode = false;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button = ensureReference<UIButton>(searchChildren:true);
        subscribeToUIButton();
        enable(HasDog);
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

		setSlot(this.dogInfo, dogSprite);
    }

	public virtual void ClearSlot()
	{
		this.dog = null;
		this.dogInfo = null;
		if(this.dogImage)
		{
        	this.dogImage.sprite = null;
        }
        enable(false);
    }

	public virtual void Init(Dog dog, bool inScoutingSelectMode)
	{
		this.inScoutingSelectMode = inScoutingSelectMode;
		this.dog = dog;
		Init(dog.Info, dog.Portrait);
	}

	public void ExecuteClick()
	{
		if(hasDogInfo && !HasDog)
		{
			this.dog = new DogFactory(hideGameObjects:true).Create(this.dogInfo);
		}
		if(HasDog)
		{
            if(showProfileOnClick)
			{
				EventController.Event(PPEvent.ClickDogSlot, this.dog);
			}
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

    protected void toggleButtonActive(bool isActive)
    {
        button.ToggleInteractable(isActive);
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

	protected virtual void callOnOccupiedSlotClick(Dog dog)
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

    protected override void enable(bool isEnabled)
    {
		if(dogImage)
		{
        	dogImage.enabled = dogImage.sprite;
		}
        if (backgroundImage)
        {
            backgroundImage.enabled = isEnabled;
        }
		if(button && dogImage)
		{
        	button.ToggleInteractable(dogImage.sprite);
		}
    }

    // Sets the dog and background sprites of this Dog Slot.
	void setSlot(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {      
        dogImage.sprite = dogSprite;
        enable(true);
        if (backgroundImage)
        {
        	backgroundImage.sprite = backgroundSprite;
		}
    }

}
