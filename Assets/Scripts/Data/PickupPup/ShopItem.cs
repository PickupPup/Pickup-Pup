/*
 * Author: Grace Barrett-Snyder
 * Description: Describes an item in the shop
 */

using UnityEngine;
using System;

[Serializable]
public class ShopItem : PPData
{
    #region Static Accessors

    public static ShopItem Default
    {
        get
        {
            ShopItem shopItem = new ShopItem();
            shopItem.itemName = string.Empty;
            return shopItem;
        }
    }

    #endregion

    #region Instance Accessors

    public int Cost
    {
        get
        {
            return cost;
        }
    }

    public string CostStr
    {
        get
        {
            return PPData.FormatCost(cost);
        }
    }

    public int Value
    {
        get
        {
            return value;
        }
    }

    public string ValueStr
    {
        get
        {
            return PPData.FormatCost(value);
        }
    }

    public string ItemName
    {
        get
        {
            return itemName;
        }
    }

    public CurrencyType CostCurrencyType
    {
        get
        {
            return costCurrencyType;
        }
    }

    public CurrencyType ValueCurrencyType
    {
        get
        {
            return valueCurrencyType;
        }
    }

    public Sprite Icon
    {
        get
        {
            if(_icon == null)
            {
                if(!spritesheetDatabase.TryGetSprite(icon, out _icon))
                {
                    _icon = DogDatabase.DefaultSprite;
                }
            }
            return _icon;
        }
    }

    #endregion

    SpritesheetDatabase spritesheetDatabase
    {
        get
        {
            if(_spritesheetDatabase == null)
            {
                _spritesheetDatabase = SpritesheetDatabase.GetInstance;
            }
            return _spritesheetDatabase;
        }
    }

    [SerializeField]
    int cost;
    [SerializeField]
    int value;
    [SerializeField]
    string itemName;
    [SerializeField]
    CurrencyType costCurrencyType;
    [SerializeField]
    CurrencyType valueCurrencyType;
    [SerializeField]
    string icon;
    Sprite _icon;
    SpritesheetDatabase _spritesheetDatabase;

    public ShopItem(
        string itemName,
        int value,
        CurrencyType valueCurrencyType,
        int cost, 
        CurrencyType costCurrencyType
        )
    {
        this.itemName = itemName;
        this.value = value;
        this.valueCurrencyType = valueCurrencyType;
        this.cost = cost;
        this.costCurrencyType = costCurrencyType;       
    }

    protected ShopItem()
    {
        // NOTHING
    }

}
