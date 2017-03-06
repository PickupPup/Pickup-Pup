/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

using UnityEngine;
using k = PPGlobal;

public class PPUIController : MonoBehaviourExtended 
{
    protected PPSceneController sceneController;
	protected PPGiftController giftController;

    protected DogProfile dogProfile;
    protected Dog selectedDog;
    protected PromptID promptID;

    [SerializeField]
    GameObject dogProfileObject;
    [SerializeField]
    GameObject dogProfileShelterObject;
    [SerializeField]
    CurrencyPanel currencyPanel;
    [SerializeField]
    NavigationPanel navigationPanel;
    [SerializeField]
    PopupPrompt popupPrompt;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        if (dogProfileObject != null)
        {
            dogProfileObject.SetActive(false);
        }
        if (dogProfileShelterObject != null)
        {
            dogProfileShelterObject.SetActive(false);
        }
        if (popupPrompt)
        {
            showPopupPrompt();
        }
    }

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		sceneController = PPSceneController.Instance;
		gameController = PPGameController.GetInstance;
		dataController = PPDataController.GetInstance;
		giftController = PPGiftController.Instance;
        setCurrencyPanel();
    }

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		EventController.Subscribe(handlePPDogEvent);
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		EventController.Unsubscribe(handlePPDogEvent);
	}

	#endregion

    public void LoadShelter()
    {
        setNavigationPanel(false);
        sceneController.LoadShelter();
    }

    public void LoadShop()
    {
        sceneController.LoadShop();
    }

	public void LoadLivingRoom()
	{
		sceneController.LoadLivingRoom();
	}

    public void LoadYard()
    {
        sceneController.LoadYard();
    }

	void handlePPDogEvent(PPEvent gameEvent, Dog dog)
	{
		if(gameEvent == PPEvent.ClickDogSlot)
		{
            selectedDog = dog;
			handleDogSlotClicked(selectedDog);
		}
	}

	void handleDogSlotClicked(Dog dog)
	{
        if (dogProfileObject)
        {
        	showDogProfile(dog);
        }
	}

    void showDogProfile(Dog dog)
    {
        EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
        

        if (dataController.AdoptedDogs.Contains(dog.Info))
        {
            dogProfile = dogProfileObject.GetComponent<DogProfile>();
            dogProfileObject.SetActive(true);
        }
        else
        {
            dogProfile = dogProfileShelterObject.GetComponent<DogProfile>();
            dogProfileShelterObject.SetActive(true);
        }

        dogProfile.Show();
        dogProfile.SetProfile(dog);
    }

    protected virtual void showPopupPrompt()
    {
        PopupPrompt prompt = (PopupPrompt) Instantiate(popupPrompt);
        prompt.GetComponent<PPUIElement>().Show();
        prompt.Set(promptID);
    }

    void setCurrencyPanel()
    {
        if (currencyPanel)
        {
			currencyPanel.Init(gameController, dataController, giftController);
        }
    }

    void setNavigationPanel(bool showAdoptButton)
    {
        if (navigationPanel)
        {
            navigationPanel.showAdoptButton(showAdoptButton);
        }
    }

}
