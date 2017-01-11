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

    #region Database Overrides

    public override void Initialize()
    {
        AssignInstance(this);
    }

    #endregion

}
