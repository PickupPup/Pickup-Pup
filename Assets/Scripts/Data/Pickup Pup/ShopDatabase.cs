/*
 * Author: Grace Barrett-Snyder
 * Description: Stores data about the shop items in the game 
 */

using UnityEngine;
using System;
using System.IO;

[Serializable]
public class ShopDatabase : Database<ShopDatabase>
{
    static Sprite defaultSprite
    {
        get
        {
            if(_defaultSprite)
            {
                return _defaultSprite;
            }
            else
            {
                // Memoization for efficiency
                _defaultSprite = Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, DEFAULT));
                return _defaultSprite;
            }
        }
    }

    static Sprite _defaultSprite;

    #region Instance Accessors

    public ShopItem[] Items
    {
        get
        {
            return items;
        }
    }

    #endregion

    [SerializeField]
    ShopItem[] items;

    public override void Initialize()
    {
        AssignInstance(this);
        populateShopItems();
    }

    void populateShopItems()
    {
        // TODO: Read this info from a JSON file in PPGameController.
        items = new ShopItem[4];
        items[0] = new ShopItem("One Dog Food Can", 1, CurrencyType.DogFood, 10, CurrencyType.Coins);
        items[1] = new ShopItem("Five Dog Food Cans", 5, CurrencyType.DogFood, 40, CurrencyType.Coins);
        items[2] = new ShopItem("Ten Dog Food Cans", 10, CurrencyType.DogFood, 70, CurrencyType.Coins);
        items[3] = new ShopItem("Twenty Dog Food Cans", 20, CurrencyType.DogFood, 100, CurrencyType.Coins);
    }

}
