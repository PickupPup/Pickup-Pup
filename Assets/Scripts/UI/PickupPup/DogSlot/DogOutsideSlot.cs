/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class DogOutsideSlot : DogSlot
{
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

	#endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite)
    {
        base.Init(dog, dogSprite);
        nameText.text = dog.Name;
    }
		
	public override void Init(Dog dog)
	{
		base.Init(dog);
		initDogScouting(dog);
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
            timerText.text = dog.TimeRemainingStr;
            dog.ResumeTimer();
        }
	}
		
	public override void ClearSlot()
	{
		// Call Dog functionality first because base method sets dog ref to null:
		dog.StopTimer();
        unsubscribeGiftEvents(dog);
		dogImage.sprite = collarSprite;
		nameText.text = string.Empty;
		timerText.text = string.Empty;
        redeemableGiftDisplay.SetActive(false);
		base.ClearSlot();
	}

	public Dog BringDogIndoors()
	{
		Dog returningDog = this.dog;
		ClearSlot();
		return returningDog;
	}

	void subscribeTimerEvents(Dog dog)
	{
		dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
		dog.SubscribeToScoutingTimerEnd(handleTimerEnd);
		scoutingDisplay.SubscribeToTimerEnd(dog);
	}

	void initDogScouting(Dog dog)
	{
		dog.TrySendToScout();
		subscribeTimerEvents(dog);
        subscribeGiftEvents(dog);
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

	void handleTimerEnd()
	{
		if(dog)
		{
     //       dog.FindGift();
		}
	}
		
}
