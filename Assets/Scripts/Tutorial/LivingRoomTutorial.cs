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
    GameObject dogCollarSlotObject;
    [SerializeField]
    GameObject dogHomeSlotObject;
    [SerializeField]
    GameObject shopButtonObject;
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
            case TutorialEvent.DogInHome:
                break;
            case TutorialEvent.Shop:
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
            case TutorialEvent.DogInHome:
                break;
            case TutorialEvent.MainMenu:
                break;
            case TutorialEvent.Shop:
                break;
            case TutorialEvent.CollarSlot:
                break;
            case TutorialEvent.SelectDogInBrowser:
                break;
            case TutorialEvent.RedeemGift:
                break;
            case TutorialEvent.Shelter:
                finish();
                break;
            default:
                base.onComplete(tutorialEvent);
                break;
        }
    }

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedLivingRoomTutorial = true;
        base.finish();
    }

}
