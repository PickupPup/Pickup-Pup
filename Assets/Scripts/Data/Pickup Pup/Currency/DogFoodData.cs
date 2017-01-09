/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's dog food currency.
 */

[System.Serializable]
public class DogFoodData : CurrencyData
{
    public DogFoodData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.DogFood;
        amount = initialAmount;
    }

}
