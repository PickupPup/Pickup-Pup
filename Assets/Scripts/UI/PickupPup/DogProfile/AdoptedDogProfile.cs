﻿/*
 * Author(s): Isaiah Mann
 * Description: Describes special behaviour only related to adopted dogs
 * Usage: [no notes]
 */

using UnityEngine;

public class AdoptedDogProfile : DogProfile 
{
    [SerializeField]
    SouvenirDisplay displaySouvenirPrefab;

    [SerializeField]
    PPUIButton souvenirButton;

    SouvenirDisplay activeSouvenirDisplay = null;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        souvenirButton.SubscribeToClick(showSouvenir);
    }

    #endregion

    void showSouvenir()
    {
        // Don't show extra copies of the souvenir
        if(activeSouvenirDisplay)
        {
            return;
        }

        SouvenirDisplay display = spawnSouvenirDisplay();
        display.Init(dogInfo.Souvenir);
    }

    SouvenirDisplay spawnSouvenirDisplay()
    {
        UIElement elem;
        if(UIElement.TryPullFromSpawnPool(typeof(SouvenirDisplay), out elem))
        {
            return elem as SouvenirDisplay;
        }
        else
        {
            return Instantiate(displaySouvenirPrefab);
        }
    }

}