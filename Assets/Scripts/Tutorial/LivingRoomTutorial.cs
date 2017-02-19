using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingRoomTutorial : Tutorial
{
    public override bool Completed
    {
        get
        {
            return PlayerPrefsUtil.CompletedLivingRoomTutorial;
        }
    }

    [SerializeField]
    GameObject currencyPanelObject;
    [SerializeField]
    GameObject dogInHome;
    [SerializeField]
    DogCollarSlot dogCollarSlot;
    [SerializeField]
    GameObject dogCollarSlotObject;
    [SerializeField]
    GameObject dogBrowserObject;
    [SerializeField]
    GameObject dogHomeSlotObject;
    [SerializeField]
    DogHomeSlot dogHomeSlot;
    [SerializeField]
    GameObject shopButtonObject;
    [SerializeField]
    UIButton shopButton;
    [SerializeField]
    GameObject shelterButtonObject;

    protected override void setReferences()
    {
        base.setReferences();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        dogHomeSlot.SubscribeToClickWhenOccupied(handleDogHomeSlotClick);
        dogCollarSlot.SubscribeToClickWhenFree(handleDogCollarSlotClick);
        shopButton.SubscribeToClick(handleShopButtonClick);
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        dogHomeSlot.UnsubscribeFromClickWhenOccupied(handleDogHomeSlotClick);
        dogCollarSlot.UnsubscribeFromClickWhenFree(handleDogCollarSlotClick);
        shopButton.UnsubscribeFromClick(handleShopButtonClick);
    }

    public override void StartTutorial()
    {
        base.StartTutorial();
        if (tutorialEvents.ContainsKey(currentTutorial) && !tutorialEvents[currentTutorial])
        {
            // Restart unfinished tutorial event
            callOnStart(currentTutorial);
        }
        else
        {
            // Default start
            callOnStart(TutorialEvent.LivingRoom, true);
        }
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.LivingRoom:
                showPopup(PromptID.ScoutingPrompt);
                break;
            case TutorialEvent.DogInHome:
                //Temp - Auto complete
                callOnComplete(TutorialEvent.DogInHome);
                // TODO: highlight dog
                break;
            case TutorialEvent.Shop:
                highlight(shopButtonObject);
                break;
            case TutorialEvent.CollarSlot: // Started in ShopTutorial
                highlight(dogCollarSlotObject);
                break;
            case TutorialEvent.SelectDogInBrowser:
                highlight(dogBrowserObject);
                highlight(dogHomeSlotObject);
                break;
            case TutorialEvent.RedeemGift:
                // TODO: Add start call
                break;
            default:
                base.onStart(tutorialEvent);
                break;
        }
    }

    protected override void onComplete(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.LivingRoom:
                callOnStart(TutorialEvent.DogInHome);
                break;
            case TutorialEvent.DogInHome:
                callOnStart(TutorialEvent.MainMenu);
                break;
            case TutorialEvent.MainMenu:
                base.onComplete(tutorialEvent);
                callOnStart(TutorialEvent.Shop);
                break;
            case TutorialEvent.Shop:
                unhighlight(shopButtonObject);
                break;
            case TutorialEvent.CollarSlot:
                unhighlight(dogCollarSlotObject);
                callOnStart(TutorialEvent.SelectDogInBrowser);
                break;
            case TutorialEvent.SelectDogInBrowser:
                unhighlight(dogBrowserObject);
                unhighlight(dogHomeSlotObject);
                break;
            case TutorialEvent.RedeemGift:
                //  TODO: Add complete call
                callOnStart(TutorialEvent.Shelter);
                break;
            case TutorialEvent.Shelter:
                finish();
                break;
            default:
                base.onComplete(tutorialEvent);
                break;
        }
    }

    protected override void handleOverlayClick()
    {
        base.handleOverlayClick();
        if (currentTutorial == TutorialEvent.DogInHome || currentTutorial == TutorialEvent.LivingRoom)
        {
            callOnComplete(currentTutorial);
        }
    }

    void handleDogHomeSlotClick(Dog dog)
    {
        if (currentTutorial == TutorialEvent.SelectDogInBrowser)
        {
            callOnComplete(currentTutorial);
        }
    }

    void handleDogCollarSlotClick()
    {
        if (currentTutorial == TutorialEvent.CollarSlot)
        {
            callOnComplete(currentTutorial);
        }
    }

    void handleShopButtonClick()
    {
        if(currentTutorial == TutorialEvent.Shop)
        {
            callOnComplete(currentTutorial);
        }
    }

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedLivingRoomTutorial = true;
        base.finish();
    }

}
