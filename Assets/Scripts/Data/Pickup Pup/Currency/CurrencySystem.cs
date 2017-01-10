/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls all forms of currency
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrencySystem : PPData, ICurrencySystem
{
    #region Static Accessors

    public static CurrencySystem Default
    {
        get
        {
            return new CurrencySystem(
                new CoinsData(2000),
                new DogFoodData(0),
                new HomeSlotsData(10)
            );
        }
    }

    #endregion

    #region ICurrencySystem Accessors

    public CoinsData Coins
    {
        get
        {
            return coins;
        }
    }

    public DogFoodData DogFood
    {
        get
        {
            return dogFood;
        }
    }

    public HomeSlotsData HomeSlots
    {
        get
        {
            return homeSlots;
        }
    }

    #endregion

    [NonSerialized]
    PPDataController dataController;

    Dictionary<CurrencyType, CurrencyData> currencies;
    CoinsData coins;
    DogFoodData dogFood;
    HomeSlotsData homeSlots;

    public CurrencySystem(CurrencyData[] currencies)
    {
        dataController = PPDataController.GetInstance;
        this.currencies = new Dictionary<CurrencyType, CurrencyData>();
        foreach(CurrencyData currency in currencies)
        {
            this.currencies.Add(currency.Type, currency);
        }
        // Set individual currencies
    }

    public CurrencySystem(CoinsData coins, DogFoodData dogFood, HomeSlotsData homeSlots)
    {
        dataController = PPDataController.GetInstance;
        this.coins = coins;
        this.dogFood = dogFood;
        this.homeSlots = homeSlots;

        generateCurrencyDictionary(new CurrencyData[3] { coins, dogFood, homeSlots });
    }

    void generateCurrencyDictionary(CurrencyData[] currencies)
    {
        this.currencies = new Dictionary<CurrencyType, CurrencyData>();
        foreach (CurrencyData currency in currencies)
        {
            this.currencies.Add(currency.Type, currency);
        }
    }

    void Save()
    {
        dataController.SaveCurrencies(this);
    }

    #region ICurrencySystem Methods

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
        Save();
    }

    public void ConvertCurrency(int value, CurrencyType valueCurrencyType, int cost, CurrencyType costCurrencyType)
    {
        if (CanAfford(costCurrencyType, cost))
        {
            ChangeCurrencyAmount(valueCurrencyType, value);
            ChangeCurrencyAmount(costCurrencyType, -cost);
        }
        // Otherwise do nothing
    }

    public bool CanAfford(CurrencyType type, int cost)
    {
        return currencies[type].CanAfford(cost);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type);
    }

    #endregion

}
