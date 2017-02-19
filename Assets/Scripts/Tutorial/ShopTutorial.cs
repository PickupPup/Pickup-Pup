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
    GameObject shopItemSlotObject;
    [SerializeField]
    GameObject livingRoomButtonObject;

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
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
    }

    public override void StartTutorial()
    {
        base.StartTutorial();
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.BuyFood:
                break;
            case TutorialEvent.CollarSlot:
                break;
            case TutorialEvent.SelectDogInBrowser:
                break;
            case TutorialEvent.RedeemGift:
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
                break;
            case TutorialEvent.CollarSlot:
                break;
            case TutorialEvent.SelectDogInBrowser:
                break;
            case TutorialEvent.RedeemGift:
                break;
            default:
                base.onComplete(tutorialEvent);
                break;
        }
    }

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedShopTutorial = true;
        base.finish();
    }

}