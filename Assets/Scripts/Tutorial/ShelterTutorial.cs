using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterTutorial : Tutorial
{
    public override bool Completed
    {
        get
        {
            return PlayerPrefsUtil.CompletedShelterTutorial;
        }
    }

    [SerializeField]
    GameObject currencyPanelObject;
    [SerializeField]
    GameObject dogShelterSlotObject;
    [SerializeField]
    Button adoptDogButton;
    [SerializeField]
    Text adoptDogButtonText;
    [SerializeField]
    Color adoptDogButtonColor;
    [SerializeField]
    Color adoptDogButtonTextColor;
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
        onStart(TutorialEvent.Shelter);
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        Debug.Log("on start: " + tutorialEvent.ToString());
        switch (tutorialEvent)
        {
            case TutorialEvent.Shelter:
                showPopup(PromptID.ShelterPrompt);
                break;
            case TutorialEvent.ShelterSlot:
                highlight(dogShelterSlotObject);
                break;
            case TutorialEvent.AdoptDog:
                adoptDogButton.image.color = adoptDogButtonColor;
                adoptDogButtonText.color = adoptDogButtonTextColor;
                break;
            case TutorialEvent.DailyGift:
                highlight(currencyPanelObject);
                break;
            default:
                base.onStart(tutorialEvent);
                break;
        }
    }

    protected override void onComplete(TutorialEvent tutorialEvent)
    {
        Debug.Log("on complete: " + tutorialEvent.ToString());
        switch (tutorialEvent)
        {
            case TutorialEvent.Shelter:
                onStart(TutorialEvent.ShelterSlot);
                break;
            case TutorialEvent.ShelterSlot:
                unhighlight(dogShelterSlotObject);
                break;
            case TutorialEvent.AdoptDog:
                break;
            case TutorialEvent.DailyGift:
                unhighlight(currencyPanelObject);
                break;
            case TutorialEvent.MainMenu:
                finish();
                break;
            default:
                base.onComplete(tutorialEvent);
                break;
        }
    }

    protected override void finish()
    {
        PlayerPrefsUtil.CompletedShelterTutorial = true;
        base.finish();
    }

}
