using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

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
    UIButton collectGiftButton;
    [SerializeField]
    GameObject dogShelterSlotParentObject;
    [SerializeField]
    DogShelterSlot[] dogShelterSlots;
    [SerializeField]
    DogShelterProfile dogShelterProfile;
    [SerializeField]
    Button adoptDogButton;
    [SerializeField]
    Text adoptDogButtonText;
    [SerializeField]
    Color adoptDogButtonColor;
    [SerializeField]
    Color adoptDogButtonTextColor;

    protected override void setReferences()
    {
        base.setReferences();
        dogShelterSlots = dogShelterSlotParentObject.GetComponentsInChildren<DogShelterSlot>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        foreach(DogSlot slot in dogShelterSlots)
        {
            slot.SubscribeToClickWhenOccupied(handleOccupiedSlotClick);
        }
        EventController.Subscribe(handleAdoptEvent);
        collectGiftButton.SubscribeToClick(handleRedeemedGift);
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        foreach(DogSlot slot in dogShelterSlots)
        {
            slot.UnsubscribeFromClickWhenOccupied(handleOccupiedSlotClick);
        }
        EventController.Unsubscribe(handleAdoptEvent);
        collectGiftButton.UnsubscribeFromClick(handleRedeemedGift);
    }

    public override void StartTutorial()
    {
        base.StartTutorial();
        callOnStart(TutorialEvent.Shelter);
    }

    protected override void onStart(TutorialEvent tutorialEvent)
    {
        switch (tutorialEvent)
        {
            case TutorialEvent.Shelter:
                showPopup(PromptID.ShelterPrompt);
                break;
            case TutorialEvent.ShelterSlot:
                highlight(dogShelterSlotParentObject);
                break;
            case TutorialEvent.AdoptDog:
                highlight(dogShelterProfile.gameObject);
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
        switch (tutorialEvent)
        {
            case TutorialEvent.Shelter:
                callOnStart(TutorialEvent.ShelterSlot);
                break;
            case TutorialEvent.ShelterSlot:
                unhighlight(dogShelterSlotParentObject);
                callOnStart(TutorialEvent.AdoptDog);
                break;
            case TutorialEvent.AdoptDog:
                unhighlight(dogShelterProfile.gameObject);
                dogShelterProfile.Hide();
                callOnStart(TutorialEvent.DailyGift);
                break;
            case TutorialEvent.DailyGift:
                unhighlight(currencyPanelObject);
                callOnStart(TutorialEvent.MainMenu);
                break;
            case TutorialEvent.MainMenu:
                callOnStart(TutorialEvent.LivingRoom);
                break;
            case TutorialEvent.LivingRoom:
                base.onComplete(tutorialEvent);
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

    protected override void handleOverlayClick()
    {
        base.handleOverlayClick();
        if(currentTutorial == TutorialEvent.Shelter)
        {
            callOnComplete(currentTutorial);
        }
    }

    void handleOccupiedSlotClick(Dog dog)
    {
        if(currentTutorial == TutorialEvent.ShelterSlot)
        {
            callOnComplete(currentTutorial);
        }
    }

    void handleAdoptEvent(string eventName, Dog dog)
    {
        if(eventName == k.ADOPT && currentTutorial == TutorialEvent.AdoptDog)
        {
            callOnComplete(TutorialEvent.AdoptDog);
        }
    }

    void handleRedeemedGift()
    {
        if(currentTutorial == TutorialEvent.DailyGift)
        {
            callOnComplete(TutorialEvent.DailyGift);
        }
    }

}
