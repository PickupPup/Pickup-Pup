/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;
using k = PPGlobal;

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
            EventController.Event(k.GetPlayEvent(k.BACK));
            Hide();
        }
        else
        {
            EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
            Show();
        }
    }

    #endregion

    public override void Hide()
    {
        EventController.Event(k.GetPlayEvent(k.BACK));
        base.Hide();
    }

    public void OnShopClick()
    {
        sceneController.LoadShop();
    }

    public void OnAllDogsClick()
    {
        if (dogBrowserObject)
        {
            EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
            dogBrowserObject.SetActive(true);
        }
    }

    public void OnLivingRoomClick()
    {
        sceneController.LoadLivingRoom();
    }

    public void OnYardClick()
    {
        sceneController.LoadYard();
    }

    public void OnSettingsClick()
    {
        EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
        settingsPopup.Show();
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
}
