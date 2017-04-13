﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's dog food currency.
 */

using System.IO;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class DogFoodData : CurrencyData
{
    public DogFoodData(int initialAmount, bool isSpecial) : base(initialAmount)
    {
        if (!isSpecial)
        {
            type = CurrencyType.DogFood;
        }
        else
        {
            type = CurrencyType.DogFoodSpecial;
        }
        amount = initialAmount;
    }

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
			return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DOG_FOOD_ICON));
        }
    }

    #endregion

}
