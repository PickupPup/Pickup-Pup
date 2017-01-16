/*
 * Author: Grace Barrett-Snyder
 * Description: Stores data about the shop items in the game 
 */

using UnityEngine;
using System;
using System.IO;
using k = PPGlobal;

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
                _defaultSprite = Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DEFAULT));
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
