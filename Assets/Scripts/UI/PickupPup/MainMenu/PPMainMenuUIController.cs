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

    PPSceneController sceneController;

    protected override void fetchReferences()
    {
        base.fetchReferences();
        sceneController = PPSceneController.Instance;       
    }

    public void Close()
    {
        Hide();
    }

    public void OnShopClick()
    {
        sceneController.LoadShop();
    }

    public void OnAllDogsClick()
    {
        if (dogBrowserObject)
        {
            dogBrowserObject.SetActive(true);
        }
    }

    public void OnSettingsClick()
    {
        settingsPopup.Show();
    }

    public void OnGiftsClick()
    {
        // Disabled
    }

    public void OnWatchAdClick()
    {
        // Disabled
    }
}
