/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the behaviour of the main menu in Pickup Pup
 * Usage: [no notes]
 */

using UnityEngine;

public class PPMainMenuUIController : PPUIController 
{
    [SerializeField]
    GameObject dogBrowserObject;

    public void Close()
    {
        gameObject.SetActive(false);
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
        // Disabled
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
