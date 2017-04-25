/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's coins currency.
 */

using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class CoinsData : CurrencyData
{
    public CoinsData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.Coins;
    }

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
			return fetchSprite(k.COIN_ICON);
        }
    }

	public override void Give()
	{
		dataController.ChangeCoins(this.Amount);
	}

    #endregion

}
