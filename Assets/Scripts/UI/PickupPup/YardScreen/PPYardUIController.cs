/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the yard screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPYardUIController : PPUIController
{
    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadYard);
    }

    #endregion

    public void OnDoorTap()
    {
        LoadLivingRoom();
    }

}
