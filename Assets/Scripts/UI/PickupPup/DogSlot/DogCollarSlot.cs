/*
 * Authors: Grace Barrett-Snyder, Ben Page
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using k = PPGlobal;

public class DogCollarSlot : DogSlot
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
    //BP radialFill holds the image that covers the button when dog is 'scouting'
    [SerializeField]
    Image radialFill;

    bool redeemDisplayIsOpen = false;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        Text[] text = GetComponentsInChildren<Text>();
        nameText = text[0];
        nameText.text = string.Empty;
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
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        if(dog)
        {
            unsubscribeFromDogEvents(dog);
        }
    }

    public override bool TryUnsubscribeAll()
    {
        unsubscribeFromUIButton();
        return true;
    }

    #endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog)
    {
        base.Init(dog);
        nameText.text = dog.Name;
        if(this.dog)
        {
            initDogScouting(this.dog, onResume:false);
        }
    }

    public override void Init(Dog dog, bool inScoutingSelectMode)
    {
        base.Init(dog, inScoutingSelectMode);
        initDogScouting(dog, onResume: false);
    }

    protected override void handleChangeDog(Dog previousDog)
    {
        base.handleChangeDog(previousDog);
        unsubscribeFromDogEvents(previousDog);
    }

    protected override void callOnOccupiedSlotClick(Dog dog)
    {
        // Safeguard against opening up tons of copies of the panel
        if(!redeemDisplayIsOpen)
        {
            base.callOnOccupiedSlotClick(dog);
        }
    }

    #endregion

    void unsubscribeFromDogEvents(Dog dog)
    {
        unsubscribeTimerEvents(dog);
        unsubscribeGiftEvents(dog);
    }

    public void ResumeScouting(Dog dog)
    {
        checkReferences();
        this.dog = dog;
        this.dogInfo = dog.Info;
        nameText.text = dog.Name;
        dogImage.sprite = dog.Portrait;
        subscribeTimerEvents(dog);
        dog.SetTimer(dogInfo.TimeRemainingScouting);
		initDogScouting(dog, onResume: true);
        if(dog.HasRedeemableGift)
        {
            handleGiftFound(dog.PeekAtGift);
        }
        else
        {
            dog.ResumeTimer();
        }
        dataController.SendDogToScout(dog);
    }

    public override void ClearSlot()
    {
        // Call Dog functionality first because base method sets dog ref to null:
        dog.StopTimer();
        unsubscribeGiftEvents(dog);
        nameText.text = string.Empty;
        redeemableGiftDisplay.SetActive(false);
        base.ClearSlot();
        dogImage.sprite = collarSprite;
        enable(true);
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
        
    void subscribeTimerEvents(Dog dog)
    {
        dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
        scoutingDisplay.SubscribeToTimerEnd(dog);
    }

    void unsubscribeTimerEvents(Dog dog)
    {
        dog.UnsubscribeFromScoutingTimerChange(handleDogTimerChange);
        scoutingDisplay.UnsubscribeFromTimerEnd(dog);
    }

    void initDogScouting(Dog dog, bool onResume)
    {
        unsubscribeFromDogEvents(dog);
        dog.TrySendToScout();
        subscribeTimerEvents(dog);
        subscribeGiftEvents(dog);
        toggleButtonActive(false);

        //BP Activate radial fill image and start lerp coroutine
        radialFill.gameObject.SetActive(true);
        float timeTotal = dog.Info.TotalTimeToReturn;
        float timeRemaining = dog.Info.TimeRemainingScouting;
        StartCoroutine(lerpRadial(timeRemaining, timeTotal));
    }

    void subscribeGiftEvents(Dog dog)
    {
        dog.SubscribeToGiftEvents(handleDogGiftEvents);
    }

    void unsubscribeGiftEvents(Dog dog)
    {
        dog.UnsubscribeFromGiftEvents(handleDogGiftEvents);
    }

    void handleDogGiftEvents(string eventName, CurrencyData gift)
    {
        switch (eventName)
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
        EventController.Event(k.GetPlayEvent(k.DOG_RETURN));
        // BP gift has been found, so deactivate the radial fill image
        if(radialFill != null)
        {
            radialFill.gameObject.SetActive(false);
        }
        toggleButtonActive(true);
        if(redeemableGiftDisplay)
        {
            redeemableGiftDisplay.SetActive(true);
        }
        if(redeemableGiftIcon)
        {
            redeemableGiftIcon.sprite = gift.Icon;
        }
    }

    void handleGiftRedeemed(CurrencyData gift)
    {
        EventController.Event(k.GetPlayEvent(k.GIFT_REDEEM));
        dog.Bark();
    }

    void handleDogTimerChange(Dog dog, float timeRemaining)
    {
        if(!dog.HasRedeemableGift)
        {
            //BP Start a lerp every second that lerps the radial fill down a second
            float totalTime = dog.Info.TotalTimeToReturn;
            StartCoroutine(lerpRadial(timeRemaining, totalTime));
            

        }
    }
    
    //BP Makes transition smooth from second to second
    IEnumerator lerpRadial(float timeRemaining, float totalTime)
    {
        float startPoint = timeRemaining / totalTime;
        float endPoint = (timeRemaining - 1) / totalTime;
        float lerpTime = 0;

        while(lerpTime < 1)
        {
            lerpTime += Time.deltaTime;
            radialFill.fillAmount = Mathf.Lerp(startPoint, endPoint, lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
