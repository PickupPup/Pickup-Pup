/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls all forms of currency
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrencySystem : PPData
{
    #region Static Accessors

    public static CurrencySystem Defaults
    {
        get
        {
            return new CurrencySystem(new CurrencyData[3]
            {
                new CoinsData(2000),
                new DogFoodData(0),
                new HomeSlotsData(10)
            });
        }
    }

    #endregion

    #region Instance Accessors

    public CurrencyData Coins
    {
        get
        {
            return dataController.Coins;
        }
    }

    public CurrencyData DogFood
    {
        get
        {
            return dataController.DogFood;
        }
    }

    public CurrencyData HomeSlots
    {
        get
        {
            return dataController.VacantHomeSlots;
        }
    }

    #endregion

    PPDataController dataController;

    Dictionary<CurrencyType, CurrencyData> currencies;
    CurrencyData coins;
    CurrencyData dogFood;
    CurrencyData homeSlots;

    public CurrencySystem(CurrencyData[] currencies)
    {
        dataController = PPDataController.GetInstance;
        this.currencies = new Dictionary<CurrencyType, CurrencyData>();
        foreach (CurrencyData currency in currencies)
        {
            this.currencies.Add(currency.Type, currency);
        }
    }

    public void ChangeCoins(int deltaCoins)
    {
        ChangeCurrencyAmount(CurrencyType.Coins, deltaCoins);
    }

    public void ChangeFood(int deltaFood)
    {
        ChangeCurrencyAmount(CurrencyType.DogFood, deltaFood);
    }
    
    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        ChangeCurrencyAmount(CurrencyType.HomeSlots, deltaHomeSlots);
    }

    public void ChangeCurrencyAmount(CurrencyType type, int deltaAmount)
    {
        currencies[type].IncreaseBy(deltaAmount);
    }

}
