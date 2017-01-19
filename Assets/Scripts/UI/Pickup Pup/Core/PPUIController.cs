/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

using UnityEngine;

public class PPUIController : MonoBehaviourExtended 
{
    protected PPSceneController sceneController;
    protected PPGameController gameController;
    protected PPDataController dataController;

    protected DogProfile dogProfile;
    protected Dog selectedDog;

    [SerializeField]
    GameObject dogProfileObject;
    [SerializeField]
    CurrencyPanel currencyPanel;
    [SerializeField]
    NavigationPanel navigationPanel;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        if (dogProfileObject != null)
        {
            dogProfileObject.SetActive(false);
        }
    }

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		sceneController = PPSceneController.Instance;
		gameController = PPGameController.GetInstance;
		dataController = PPDataController.GetInstance;

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

    public void LoadMainMenu() 
	{
		sceneController.LoadMainMenu();
	}

	public void LoadHome()
	{
		sceneController.LoadHome();
	}

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
        // TODO: Insert universal dog slot handle code here
        if (dogProfileObject)
        {
            // TEMP until scouting code is finished
            if (sceneController.CurrentScene == PPScene.Shelter)
            {
                showDogProfile(dog);
            }
        }
	}

    void showDogProfile(Dog dog)
    {
        EventController.Event("PlayMenuPopup");
        dogProfileObject.SetActive(true);
        if(!dogProfile)
        {
            dogProfile = dogProfileObject.GetComponent<DogProfile>();
        }
        dogProfile.SetProfile(dog);
    }

    void setCurrencyPanel()
    {
        if (currencyPanel)
        {
            currencyPanel.Init(dataController);
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
