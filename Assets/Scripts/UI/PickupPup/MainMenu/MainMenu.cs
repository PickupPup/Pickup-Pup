/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder, Ben Page
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

using k = PPGlobal;

public class MainMenu : PPUIElement
{
    const int ALT_SINGLE_DROPDOWN = k.ALT_SINGLE_DROPDOWN;

	[Header("External References")]
    [SerializeField]
    DogBrowser dogBrowser;
    [SerializeField]
    SettingsMenu settingsMenu;

	[Header("Internal References")]
	[SerializeField]
	GameObject dogDropdown;
	[SerializeField]
	GameObject settingsDropdown;
    [SerializeField]
    PPUIButton dynamicNavButton;
    [SerializeField]
    PPUIButton dogsButton;
    [SerializeField]
    PPUIButton settingsButton;

    [SerializeField]
    GameObject altNavPanel;
    [SerializeField]
    Image dynamicNavButtonBackground;
    [SerializeField]
    GameObject singleDropdown;
    [SerializeField]
    GameObject singledDropdownArrow;

    [Header("Serialized Sprites")]
    [SerializeField]
    Sprite homeIcon;
    [SerializeField]
    Sprite shelterIcon;
    [SerializeField]
    Sprite shopIcon;
    [SerializeField]
    Sprite dogsIcon;
    [SerializeField]
    Sprite allDogsIcon;
    [SerializeField]
    Sprite roundButtonBackground;

    [Header("Color Palette")]
    [SerializeField]
    Color buttonColor;

    [Header("Tuning Values")]
    [SerializeField]
    Vector3 navButtonRotation = new Vector3(180, 0);

    [Header("Debugging")]
    [SerializeField]
    bool setAltSingleDropdownOn;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        setupDynamicButtons();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        sceneController = PPSceneController.Instance;
    }

    protected override void handleSceneLoaded (PPScene scene)
    {
        base.handleSceneLoaded(scene);
        updateDynamicNavButton(scene);
    }

    #endregion

    #region UIElement Overrides

    public override void Toggle()
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
        if (gameController)
        {
            gameController.ToggleMainMenuOpen(gameObject.activeSelf);
        }
    }

    #endregion

    public override void Hide()
    {
        base.Hide();
        EventController.Event(k.GetPlayEvent(k.BACK));
    }

    public override void Show()
    {
        base.Show();
        EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
    }

    public void OnShopClick()
    {
        sceneController.LoadShop();
    }

    public void OnAllDogsClick()
    {
        if(dogBrowser)
        {
            EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
            dogBrowser.Open(inScoutingSelectMode:false);
        }
        ToggleDogDropdown();
        dogsButton.SetImage(allDogsIcon);
    }

    public void OnHomeClick()
    {
        sceneController.LoadHome();
    }

    public void OnSettingsClick()
    {
        settingsMenu.Toggle();
        if(settingsMenu.IsVisible)
        {
            EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
        }
        ToggleSettingsDropdown();
    }

    public void OnShelterClick()
    {
        sceneController.LoadShelter();
    }

    public void OnGiftsClick()
    {
        // Disabled
        EventController.Event(k.GetPlayEvent(k.EMPTY));
    }

    public void OnWatchAdClick()
    {
        // Disabled
        EventController.Event(k.GetPlayEvent(k.EMPTY));
    }

	public void ToggleDogDropdown()
	{
		dogDropdown.SetActive(!dogDropdown.activeSelf);
	}

	public void ToggleSettingsDropdown()
	{
		settingsDropdown.SetActive(!settingsDropdown.activeSelf);
	}

    public void ToggleSingleNavDropdown()
    {
        singleDropdown.SetActive(!singleDropdown.activeSelf);
        singledDropdownArrow.transform.Rotate(navButtonRotation);
    }

    void showCorrectNavPanel()
    {
        if(SettingsUtil.NavDropDownType == ALT_SINGLE_DROPDOWN || setAltSingleDropdownOn)
        {
            switchToSingleDropdown();
        }
    }

    void switchToSingleDropdown()
    {
        dynamicNavButtonBackground.sprite = roundButtonBackground;
        dynamicNavButtonBackground.color = buttonColor;
        dogsButton.Hide();
        settingsButton.Hide();
        altNavPanel.SetActive(true);
    }
        
    void setupDynamicButtons()            
    {
        dynamicNavButton.SetImageToChild();
        dogsButton.SetImageToChild();
    }

    void updateDynamicNavButton(PPScene currentScene)
    {
        dynamicNavButton.TryUnsubscribeAll();
        if(currentScene == PPScene.Home)
        {
            dynamicNavButton.SubscribeToClick(OnShelterClick);
            dynamicNavButton.SetImage(shelterIcon);
        }
        else
        {
            dynamicNavButton.SubscribeToClick(OnHomeClick);
            dynamicNavButton.SetImage(homeIcon);
            if(currentScene == PPScene.Shop)
            {
                dogsButton.SetImage(shopIcon);
            }
        }
    }

}
