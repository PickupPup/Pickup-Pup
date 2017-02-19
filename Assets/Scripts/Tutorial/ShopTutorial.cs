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
        callOnStart(TutorialEvent.BuyFood);
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.BuyFood:
                highlight(shopItemSlotObject);
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
            case TutorialEvent.BuyFood:
                unhighlight(shopItemSlotObject);
                callOnStart(TutorialEvent.MainMenu);
                break;
            case TutorialEvent.MainMenu:
                base.onComplete(TutorialEvent.MainMenu);
                callOnStart(TutorialEvent.LivingRoom);
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

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedShopTutorial = true;
        callOnStart(TutorialEvent.CollarSlot);
        base.finish();
    }

}