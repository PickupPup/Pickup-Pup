/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the shop screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPShopUIController : PPUIController
{
    [SerializeField]
    CurrencyDisplay dogFoodDisplay;
    [SerializeField]
    CurrencyDisplay coinDisplay;

    ShopDatabase shop;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        //EventController.Event(PPEvent.LoadHome); // TODO: Change this
        shop = ShopDatabase.Instance;
        shop.Initialize();

        // Set Currency Displays
        dogFoodDisplay.SetCurrency(gameController.DogFood);
        coinDisplay.SetCurrency(gameController.Coins);
    }

    #endregion

    void populateShop(ShopDatabase shop)
    {
        // TODO
    }

}
