﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's dog food currency.
 */

using UnityEngine;

[System.Serializable]
public class DogFoodData : CurrencyData
{
    public DogFoodData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.DogFood;
        amount = initialAmount;
    }

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
            return base.Icon;
        }
    }

    #endregion

}