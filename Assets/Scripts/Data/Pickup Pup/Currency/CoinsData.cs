/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's coins currency.
 */

using UnityEngine;

[System.Serializable]
public class CoinsData : CurrencyData
{
    public CoinsData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.Coins;
        amount = initialAmount;
    }

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
            Sprite icon;
            spriteDatabase.TryGetSprite("Coin", out icon);
            return icon;
        }
    }

    #endregion

}
