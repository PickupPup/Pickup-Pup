/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls navigation between Dog Profiles
 */

using System.Collections.Generic;
using UnityEngine;

public class DogProfileButtonController : PPUIButtonController
{
    #region Instance Accessors

    public bool IsInitialized
    {
        get;
        private set;
    }

    #endregion

    [SerializeField]
    UIButton pageBackwardButton;
    [SerializeField]
    UIButton pageForwardButton;

    DogProfile parentWindow;
    ToggleableColorUIButton[] pageButtons;
    ToggleableColorUIButton selectedPageButton;

    List<Dog> dogsList;
    int currentProfileIndex;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        // GetComponentInParent also checks the current GameObject (as per the current prefab)
        parentWindow = GetComponentInParent<DogProfile>();
    }

    #endregion

    public void Init(DogProfile profile, List<Dog> dogsList)
    {
        this.parentWindow = profile;
        this.dogsList = dogsList;
        IsInitialized = true;

        pageBackwardButton.SubscribeToClick(previousProfile);
        pageForwardButton.SubscribeToClick(nextProfile);
    }

    void switchToProfile(int index)
    {
        checkReferences();
        currentProfileIndex = index;
        checkCurrentIndex();
        parentWindow.SetProfile(dogsList[currentProfileIndex]);
    }

    void nextProfile()
    {
        switchToProfile(currentProfileIndex + 1);
    }

    void previousProfile()
    {
        switchToProfile(currentProfileIndex - 1);
    }

    bool checkCurrentIndex()
    {
        if(currentProfileIndex >= 0 && currentProfileIndex < dogsList.Count)
        {
            fixCurrentIndex();
            return false;
        }
        return true;
    }

    void fixCurrentIndex()
    {
        if(currentProfileIndex < 0)
        {
            currentProfileIndex = dogsList.Count - 1;
        }
        else if(currentProfileIndex >= dogsList.Count)
        {
            currentProfileIndex = currentProfileIndex = 0;
        }
    }

}
