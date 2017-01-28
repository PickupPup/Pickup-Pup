/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class DogOutsideSlot : DogSlot
{
	// Disable this because it conflicts w/ scouting
	protected override bool showProfileOnClick
	{
		get
		{
			return false;
		}
	}

	ScoutingDisplay scoutingDisplay;

    Text nameText;
    Text timerText;
	[SerializeField]
	Image dogImageOverride;
    [SerializeField]
    Image redeemableGiftIcon;
    [SerializeField]
    GameObject redeemableGiftDisplay;
	[SerializeField]
	Sprite collarSprite;

    bool redeemDisplayIsOpen = false;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		Text[] text = GetComponentsInChildren<Text>();
		nameText = text[0];
		timerText = text[1];
		nameText.text = string.Empty;
		timerText.text = string.Empty;
		scoutingDisplay = GetComponentInParent<ScoutingDisplay>();
	}

	protected override void checkReferences()
	{
		base.checkReferences();
		if(dogImageOverride)
		{
			dogImage = dogImageOverride;
		}
	}

	protected override void handleSceneLoaded(int sceneIndex)
	{
		base.handleSceneLoaded(sceneIndex);
		if(!timerText)
		{
			timerText = gameObject.AddComponent<Text>();
		}
	}
		
	protected override void cleanupReferences()
	{
		base.cleanupReferences();
		if(dog)
		{
			dog.UnsubscribeFromScoutingTimerChange(handleDogTimerChange);
		}
	}

    public override bool TryUnsubscribeAll()
    {
        unsubscribeFromUIButton();
        return true;
    }

	#endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite)
    {
        base.Init(dog, dogSprite);
        nameText.text = dog.Name;
    }
		
	public override void Init(Dog dog, bool inScoutingSelectMode)
	{
        initDogScouting(dog, onResume:false);
		base.Init(dog, inScoutingSelectMode);
        dataController.SaveGame();
    }

    #endregion

    #region UIElement Overrides 

    public override void SetText (string text)
    {
        timerText.text = text;
    }

    #endregion

	public void ResumeScouting(Dog dog)
	{
		checkReferences();
		this.dog = dog;
		this.dogInfo = dog.Info;
		nameText.text = dog.Name;
		dogImage.sprite = dog.Portrait;
        subscribeTimerEvents(dog);
        dog.SetTimer(dogInfo.TimeRemainingScouting);
        if(dog.HasRedeemableGift)
        {
            handleGiftFound(dog.PeekAtGift);
        }
        else
        {
            initDogScouting(dog, onResume:true);
            timerText.text = dog.TimeRemainingStr;
            dog.ResumeTimer();
        }
        dataController.SaveGame();
	}
		
	public override void ClearSlot()
	{
		// Call Dog functionality first because base method sets dog ref to null:
		dog.StopTimer();
        unsubscribeGiftEvents(dog);
		nameText.text = string.Empty;
		timerText.text = string.Empty;
        redeemableGiftDisplay.SetActive(false);
		base.ClearSlot();
        dogImage.sprite = collarSprite;
	}

	public Dog BringDogIndoors()
	{
		Dog returningDog = this.dog;
		ClearSlot();
		return returningDog;
	}


    public void ToggleRedeemDisplayOpen(bool isOpen)
    {
        this.redeemDisplayIsOpen = isOpen;
    }

    protected override void callOnOccupiedSlotClick (Dog dog)
    {
        // Safeguard against opening up tons of copies of the panel
        if(!redeemDisplayIsOpen)
        {
            base.callOnOccupiedSlotClick (dog);
        }
    }

	void subscribeTimerEvents(Dog dog)
	{
		dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
		scoutingDisplay.SubscribeToTimerEnd(dog);
	}

    void initDogScouting(Dog dog, bool onResume)
	{
        if(!onResume)
        {
		    dog.TrySendToScout();
        }
		subscribeTimerEvents(dog);
        subscribeGiftEvents(dog);
        toggleButtonActive(false);
	}
        
    void subscribeGiftEvents(Dog dog)
    {
        dog.SubscribeToGiftEvents(handleDogGiftEvents);
    }

    void unsubscribeGiftEvents(Dog dog)
    {
        dog.UnsusbscribeFromGiftEvents(handleDogGiftEvents);
    }

    void handleDogGiftEvents(string eventName, CurrencyData gift)
    {
        switch(eventName)
        {
            case FIND_GIFT:
                handleGiftFound(gift);
                break;
            case REDEEM_GIFT:
                handleGiftRedeemed(gift);
                break;
        }
    }

    void handleGiftFound(CurrencyData gift)
    {
        toggleButtonActive(true);
        redeemableGiftDisplay.SetActive(true);
        redeemableGiftIcon.sprite = gift.Icon;
        timerText.text = languageDatabase.GetTerm(TAP_TO_REDEEM);
    }

    void handleGiftRedeemed(CurrencyData gift)
    {
        EventController.Event(k.GetPlayEvent(k.GIFT_REDEEM));
        EventController.Event(k.GetPlayEvent(k.BARK), dogInfo.Breed.Size);
    }

	void handleDogTimerChange(Dog dog, float timeRemaining)
	{
        if(timerText && !dog.HasRedeemableGift)
		{
			timerText.text = dog.RemainingTimeScoutingStr;	
		}
	}
		
}
