﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: An interface to facilitate data flow of currency changes
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
	void GiveCurrency(CurrencyData currency);
	bool TryTakeCurrency(CurrencyData currency);
    bool CanAfford(CurrencyType type, int amount);
    bool HasCurrency(CurrencyType type);

}
