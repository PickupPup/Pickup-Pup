/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;

public class PPMainMenuUIController : PPUIElement 
{
    [SerializeField]
    GameObject dogBrowserObject;
    [SerializeField]
    SettingsPopup settingsPopup;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        sceneController = PPSceneController.Instance;
    }

    #endregion

    #region UIElement Overrides

    public override void Toggle()
    {
        if(gameObject.activeSelf)
        {
            EventController.Event("PlayBack");
            Hide();
        }
        else
        {
            EventController.Event("PlayMenuPopup");
            Show();
        }
    }

    #endregion

    public void Close()
    {
        EventController.Event("PlayBack");
        Hide();
    }

    public void OnShopClick()
    {
        EventController.Event("PlayMenuClick");
        sceneController.LoadShop();
    }

    public void OnAllDogsClick()
    {
        if (dogBrowserObject)
        {
            EventController.Event("PlayMenuPopup");
            dogBrowserObject.SetActive(true);
        }
    }

    public void OnLivingRoomClick()
    {
        EventController.Event("PlayMenuClick");
        sceneController.LoadLivingRoom();
    }

    public void OnYardClick()
    {
        EventController.Event("PlayMenuClick");
        sceneController.LoadYard();
    }

    public void OnSettingsClick()
    {
        EventController.Event("PlayMenuClick");
        settingsPopup.Show();
    }

    public void OnGiftsClick()
    {
        // Disabled
        EventController.Event("PlayEmpty");
    }

    public void OnWatchAdClick()
    {
        // Disabled
        EventController.Event("PlayEmpty");
    }
}
