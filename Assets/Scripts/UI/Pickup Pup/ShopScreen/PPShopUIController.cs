﻿/*
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

    ShopItem[] items;
    ShopItemSlot[] itemSlots;
    ShopDatabase shop;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        EventController.Event(PPEvent.LoadShop);
        itemSlots = GetComponentsInChildren<ShopItemSlot>();

    }

    protected override void fetchReferences()
    {
        base.fetchReferences();

        shop = ShopDatabase.Instance;
        shop.Initialize();
        items = shop.Items;
        populateShop(items);
    }

    #endregion

    void populateShop(ShopItem[] items)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ShopItemSlot itemSlot = itemSlots[i];
            itemSlot.Init(this, items[i]);
        }
    }

    public void OnMenuClick()
    {
		sceneController.LoadMainMenu();
    }

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

}
