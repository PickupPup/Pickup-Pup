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
    [SerializeField]
    PPMainMenuUIController mainMenu;

    PPSceneController sceneController;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        sceneController = PPSceneController.Instance;
    }

    #endregion

    public void showAdoptButton(bool show)
    {
        if (adoptButton)
        {
            adoptButton.interactable = show;
        }
    }

    public void OnMenuClick()
    {
        mainMenu.Toggle();
    }

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

}
