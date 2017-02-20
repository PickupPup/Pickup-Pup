using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorial : Tutorial
{
    public override bool Completed
    {
        get
        {
            return PlayerPrefsUtil.CompletedShopTutorial;
        }
    }

    [SerializeField]
    UIButton shopItemSlotButton;
    [SerializeField]
    GameObject shopItemSlotObject;

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
        shopItemSlotButton.SubscribeToClick(handleShopButtonClick);
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        shopItemSlotButton.UnsubscribeFromClick(handleShopButtonClick);
    }

    public override void StartTutorial()
    {
        base.StartTutorial();
        callOnStart(TutorialEvent.Shop);
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.BuyFood:
                highlight(shopItemSlotObject);
                break;
            case TutorialEvent.Shop:
                showPopup(PromptID.ShelterPrompt);
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
            case TutorialEvent.Shop:
                callOnStart(TutorialEvent.BuyFood);
                break;
            case TutorialEvent.BuyFood:
                unhighlight(shopItemSlotObject);
                callOnStart(TutorialEvent.MainMenu, true);
                break;
            case TutorialEvent.MainMenu:
                base.onComplete(TutorialEvent.MainMenu);
                callOnStart(TutorialEvent.LivingRoom, true);
                break;
            case TutorialEvent.LivingRoom:
                finish();
                break;
            default:
                base.onComplete(tutorialEvent);
                break;
        }
    }

    void handleShopButtonClick()
    {
        if(currentTutorial == TutorialEvent.BuyFood)
        {
            callOnComplete(TutorialEvent.BuyFood);
        }
    }

    protected override void handleOverlayClick()
    {
        base.handleOverlayClick();
        if (currentTutorial == TutorialEvent.Shop)
        {
            callOnComplete(currentTutorial);
        }
    }

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedShopTutorial = true;
        callOnStart(TutorialEvent.CollarSlot);
        base.finish();
    }

}