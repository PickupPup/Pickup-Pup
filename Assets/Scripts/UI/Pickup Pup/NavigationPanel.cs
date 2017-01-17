/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the navigation buttons (menu and adopt) on every screen
 */

using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : SingletonController<NavigationPanel>
{
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Button adoptButton;

    public void showAdoptButton(bool show)
    {
        if (adoptButton)
        {
            adoptButton.interactable = show;
        }
    }

}
