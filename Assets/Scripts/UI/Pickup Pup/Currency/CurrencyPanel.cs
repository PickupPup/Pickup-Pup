/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the display of multiple currencies on the Currency Panel
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPanel : PPUIElement
{
    [SerializeField]
    CurrencyDisplay coinDisplay;
    [SerializeField]
    CurrencyDisplay dogFoodDisplay;

    protected override void fetchReferences()
    {
        base.fetchReferences();
        PPDataController dataController = PPDataController.GetInstance;

        // Display Updated Currency
        dogFoodDisplay.Init(dataController, dataController.DogFood);
        coinDisplay.Init(dataController, dataController.Coins);
    }

}
