/*
 * Author: Grace Barrett-Snyder, Ben Page
 * Description: Controls the navigation buttons (menu and adopt) on every screen
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class NavigationPanel : SingletonController<NavigationPanel>
{
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Button adoptButton;
    [SerializeField]
    MainMenu mainMenu;
    [SerializeField]
    DogProfile dogProfile;
    [SerializeField]
    DogBrowser dogBrowser;

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
        if(dogProfile && dogProfile.isActiveAndEnabled)
        {
            dogProfile.Hide();
        }
        if(dogBrowser.isActiveAndEnabled)
        {
            dogBrowser.Hide();
        }
        
        
    }

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

}
