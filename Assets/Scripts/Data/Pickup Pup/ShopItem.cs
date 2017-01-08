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
            return formatCost(cost);
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
            return formatCost(value);
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

    #endregion

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

    public ShopItem()
    {
        // NOTHING
    }

}
