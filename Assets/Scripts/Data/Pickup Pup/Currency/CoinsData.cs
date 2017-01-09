/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's coins currency.
 */

[System.Serializable]
public class CoinsData : CurrencyData
{
    public CoinsData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.Coins;
        amount = initialAmount;
    }

}
