using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviourExtended
{
    public virtual bool Completed
    {
        get
        {
            return completed;
        }
    }

    UICanvas canvas;
    [SerializeField]
    PopupPrompt popupPrompt;
    [SerializeField]
    UIButton overlay;
    [SerializeField]
    GameObject navigationPanelObject;
    [SerializeField]
    UIButton menuNavButton;
    [SerializeField]
    UIButton shelterNavButton;
    [SerializeField]
    UIButton livingRoomButton;

    Dictionary<GameObject, Transform> highlightedObjects;
    protected static Dictionary<TutorialEvent, bool> tutorialEvents; // Value is true if the tutorial has been completed
    protected static TutorialEvent currentTutorial;

    bool completed;

    protected override void setReferences()
    {
        base.setReferences();
        canvas = GetComponent<UICanvas>();
        highlightedObjects = new Dictionary<GameObject, Transform>();
        tutorialEvents = new Dictionary<TutorialEvent, bool>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        overlay.SubscribeToClick(handleOverlayClick);
        menuNavButton.SubscribeToClick(handleMenuButtonClick);
        livingRoomButton.SubscribeToClick(handleLivingRoomButtonClick);
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        overlay.UnsubscribeFromClick(handleOverlayClick);
        menuNavButton.UnsubscribeFromClick(handleMenuButtonClick);
        livingRoomButton.UnsubscribeFromClick(handleLivingRoomButtonClick);
    }

    public virtual void StartTutorial()
    {
        completed = false;
    }

    protected void callOnStart(TutorialEvent tutorialEvent, bool forceStart = false)
    {       
        if(!tutorialEvents.ContainsKey(tutorialEvent))
        {
            tutorialEvents.Add(tutorialEvent, false);
        }
        if(forceStart || !tutorialEvents[tutorialEvent])
        {
            currentTutorial = tutorialEvent;
            onStart(tutorialEvent);
            Debug.Log("on start: " + tutorialEvent.ToString());
        }
    }

    protected virtual void onStart(TutorialEvent tutorialEvent)
    {
        switch(tutorialEvent)
        {
            case TutorialEvent.MainMenu:
                showNavPanel(true, false);
                break;
            case TutorialEvent.LivingRoom:
                highlight(livingRoomButton.gameObject);
                break;
            case TutorialEvent.Shelter:
                showNavPanel(false, true);
                break;
        }
    }

    protected void callOnComplete(TutorialEvent tutorialEvent)
    {
        onComplete(tutorialEvent);
        if (tutorialEvents.ContainsKey(tutorialEvent))
        {
            tutorialEvents[tutorialEvent] = true;
            Debug.Log("on complete: " + tutorialEvent.ToString());
        }
    }

    protected virtual void onComplete(TutorialEvent tutorialEvent)
    {
        switch(tutorialEvent)
        {
            case TutorialEvent.MainMenu:
                unhighlight(navigationPanelObject);              
                break;
            case TutorialEvent.LivingRoom:
                unhighlight(livingRoomButton.gameObject);
                break;
            case TutorialEvent.Shelter:            
                break;
        }
    }

    protected void showPopup(PromptID promptID)
    {
        overlay.Show();
        Debug.Log("showing popup");
        popupPrompt.GetComponent<PPUIElement>().Show();
        popupPrompt.Set(promptID);
    }

    protected void highlight(GameObject gameObject)
    {
        overlay.Show();
        highlightedObjects.Add(gameObject, gameObject.transform.parent);
        gameObject.transform.SetParent(transform);
    }

    protected void unhighlight(GameObject gameObject)
    {
        if(highlightedObjects.ContainsKey(gameObject))
        {
            gameObject.transform.SetParent(highlightedObjects[gameObject]);
            overlay.Hide();
        }
    }

    void showNavPanel(bool enableMenuButton, bool enableShelterButton)
    {
        highlight(navigationPanelObject);
        menuNavButton.enabled = enableMenuButton;
        shelterNavButton.enabled = enableShelterButton;
    }

    protected virtual void finish()
    {
        unsubscribeEvents();
        completed = true;
        canvas.Hide();
    }

    protected virtual void handleOverlayClick()
    {
        TryClosePopup();
    }

    protected virtual void handleMenuButtonClick()
    {
        if (currentTutorial == TutorialEvent.MainMenu)
        {
            callOnComplete(TutorialEvent.MainMenu);
        }
    }

    void handleLivingRoomButtonClick()
    {
        if (currentTutorial == TutorialEvent.LivingRoom)
        {
            callOnComplete(TutorialEvent.LivingRoom);
        }
    }

    public void TryClosePopup()
    {
        if(popupPrompt.isActiveAndEnabled)
        {
            popupPrompt.Hide();
            overlay.Hide();
        }
    }

}

public enum TutorialEvent
{
    ShelterSlot,
    AdoptDog,
    DailyGift,
    MainMenu,
    LivingRoom,
    DogInHome,
    Shop,
    BuyFood,
    CollarSlot,
    SelectDogInBrowser,
    RedeemGift,
    Shelter
}
