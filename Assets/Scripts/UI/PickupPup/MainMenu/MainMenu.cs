/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder, Ben Page
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

using k = PPGlobal;
using System.Collections;

public class MainMenu : PPUIElement
{
    const int STANDARD_DROPDOWN = k.STANDARD_DROPDOWN;
    const int ALT_SINGLE_DROPDOWN = k.ALT_SINGLE_DROPDOWN;

    const string ANDROID_ID = "1347448";
    const string IOS_ID = "1347449";

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
    [SerializeField]
    Sprite rectButtonBackground;

    [Header("Color Palette")]
    [SerializeField]
    Color buttonColor;

    [Header("Tuning Values")]
    [SerializeField]
    Vector3 navButtonRotation = new Vector3(180, 0);

    [Header("Debugging")]
    [SerializeField]
    bool setAltSingleDropdownOn;

    int currentNavPanelType = STANDARD_DROPDOWN;
        
    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        showCorrectNavPanel();
        setupDynamicButtons();
        SettingsUtil.SubscribeToNavPanelChange(showCorrectNavPanel);
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

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        SettingsUtil.UnsubscribeFromNavPanelChange(showCorrectNavPanel);
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
        ToggleSettingsDropdown();
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

        #if UNITY_IOS
                Advertisement.Initialize(IOS_ID, true);
        #endif

        #if UNITY_ANDROID
                Advertisement.Initialize(ANDROID_ID, true);
        #endif

        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady("rewardedVideo"))
            yield return null;

        Advertisement.Show("rewardedVideo", new ShowOptions()
        {
            resultCallback = result =>
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        Debug.Log("Advertisement Finish");
                        break;
                    case ShowResult.Failed:
                        Debug.Log("Advertisement Failed");
                        break;
                    case ShowResult.Skipped:
                        Debug.Log("Advertisement Skipped");
                        break;
                    default:
                        break;
                }
            }

        });
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
        else
        {
            switchToStandardNav();
        }
    }

    void switchToSingleDropdown()
    {
        if(currentNavPanelType != ALT_SINGLE_DROPDOWN)
        {
            dynamicNavButtonBackground.sprite = roundButtonBackground;
            dynamicNavButtonBackground.color = buttonColor;
            dogsButton.Hide();
            settingsButton.Hide();
            altNavPanel.SetActive(true);
            currentNavPanelType = ALT_SINGLE_DROPDOWN;
        }
    }
     
    void switchToStandardNav()
    {
        if(currentNavPanelType != STANDARD_DROPDOWN)
        {
            dynamicNavButtonBackground.sprite = rectButtonBackground;
            dynamicNavButtonBackground.color = Color.white;
            dogsButton.Show();
            settingsButton.Show();
            altNavPanel.SetActive(false);
            currentNavPanelType = STANDARD_DROPDOWN;
        }
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
