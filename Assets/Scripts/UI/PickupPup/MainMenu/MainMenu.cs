/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder, Ben Page, Timothy Ng
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using k = PPGlobal;

public class MainMenu : PPUIElement
{
	[Header("External References")]
    [SerializeField]
    DogBrowser dogBrowser;
    [SerializeField]
    GameObject settingsMenuPrefab;
    SettingsMenu settingsMenu;

	[Header("Internal References")]
    [SerializeField]
    PPUIButton dynamicNavButton;

    [SerializeField]
    Image dynamicNavButtonBackground;
    [SerializeField]
    GameObject singleDropdown;
    [SerializeField]
    GameObject singleDropdownArrow;

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
        
    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        dynamicNavButtonBackground.sprite = roundButtonBackground;
        dynamicNavButtonBackground.color = buttonColor;
        setupDynamicButtons();
        setupSettingsMenu();
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
        ToggleSingleNavDropdown();
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
        ToggleSingleNavDropdown();
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
        AdController.GetInstance.WatchAd();
    }

    public void OnIAPClick()
    {
        sceneController.LoadIAP();
    }

    public void ToggleSingleNavDropdown()
    {
        singleDropdown.SetActive(!singleDropdown.activeSelf);
        singleDropdownArrow.transform.Rotate(navButtonRotation);
    }

    void setupSettingsMenu()
    {
        GameObject settingsMenuObject = (GameObject) Instantiate(settingsMenuPrefab);
        settingsMenu = settingsMenuObject.GetComponent<SettingsMenu>();
    }

    void setupDynamicButtons()            
    {
        dynamicNavButton.SetImageToChild();
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
        }
    }

}
