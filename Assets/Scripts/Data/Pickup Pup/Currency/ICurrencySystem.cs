/*
 * Author: Grace Barrett-Snyder 
 * Description: An interface for every class that manages currency changes
 */

public interface ICurrencySystem
{
    #region Instance Accessors

    CoinsData Coins
    {
        get;
    }

    DogFoodData DogFood
    {
        get;
    }

    HomeSlotsData HomeSlots
    {
        get;
    }

    #endregion

    void ChangeCoins(int deltaCoins);
    void ChangeFood(int deltaFood);
    void ChangeHomeSlots(int deltaHomeSlots);
    void ChangeCurrencyAmount(CurrencyType type, int deltaAmount);
    void ConvertCurrency(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType);
    bool CanAfford(CurrencyType type, int amount);
    bool HasCurrency(CurrencyType type);

}
