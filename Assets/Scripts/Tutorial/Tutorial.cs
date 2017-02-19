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

    [SerializeField]
    PopupPrompt popupPrompt;
    [SerializeField]
    Image overlay;
    [SerializeField]
    GameObject navigationPanelObject;
    [SerializeField]
    UIButton menuNavButton;
    [SerializeField]
    UIButton shelterNavButton;

    bool completed;

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

    public virtual void StartTutorial()
    {
        completed = false;
    }

    protected virtual void onStart(TutorialEvent tutorialEvent)
    {
        switch(tutorialEvent)
        {
            case TutorialEvent.MainMenu:
                showNavPanel(true, false);
                break;
            case TutorialEvent.LivingRoom:
                break;
            case TutorialEvent.Shelter:
                showNavPanel(false, true);
                break;
        }
    }

    protected virtual void onComplete(TutorialEvent tutorialEvent)
    {
        switch(tutorialEvent)
        {
            case TutorialEvent.MainMenu:               
                break;
            case TutorialEvent.LivingRoom:
                break;
            case TutorialEvent.Shelter:            
                break;
        }
    }

    protected void showPopup(PromptID promptID)
    {
        PopupPrompt prompt = (PopupPrompt) Instantiate(popupPrompt);
        prompt.GetComponent<PPUIElement>().Show();
        prompt.Set(promptID);
    }

    protected void highlight(GameObject gameObject)
    {
        // TODO
    }

    protected void unhighlight(GameObject gameObject)
    {
        // TODO
    }

    void showNavPanel(bool enableMenuButton, bool enableShelterButton)
    {
        highlight(navigationPanelObject);
        menuNavButton.enabled = enableMenuButton;
        shelterNavButton.enabled = enableShelterButton;
    }

    protected void showOverlay(bool show)
    {
        overlay.enabled = show;
    }

    protected virtual void finish()
    {
        unsubscribeEvents();
        completed = true;
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
