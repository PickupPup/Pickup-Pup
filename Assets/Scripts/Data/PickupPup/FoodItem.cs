/*
 * Author: Grace Barrett-Snyder, Ben Page
 * Description: Stores Food information - based off ShopItem.
 */

using UnityEngine;
using System;

[Serializable]
public class FoodItem : PPData
{
    #region Static Accessors

    public static FoodItem Default
    {
        get
        {
            FoodItem foodItem = new FoodItem();
            foodItem.foodName = string.Empty;
            return foodItem;
        }
    }

    #endregion

    #region Instance Accessors

    public DogFoodType FoodType
    {
        get
        {
            return type;
        }
    }

    public string FoodName
    {
        get
        {
            return foodName;
        }
    }
    
    public float SpecialGiftMod
    {
        get
        {
            return specialGiftMod;
        }
    }

    public int AmounntMod
    {
        get
        {
            return amountMod;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
    }

    public int StartingAmount
    {
        get
        {
            return startingAmount;
        }
    }

    public int CurrentAmount
    {
        get
        {
            return currentAmount;
        }
        set
        {
            currentAmount = value;
        }
    }

    #endregion

    DogFoodType type;
    string foodName;
    float specialGiftMod;
    int amountMod;
    Color color;
    int startingAmount;
    int currentAmount;

    public FoodItem(
        DogFoodType type,
        string foodName,
        float specialGiftMod,
        int amountMod,
        Color color,
        int startingAmount,
        int currentAmount
        )
    {
        this.foodName = foodName;
        this.specialGiftMod = specialGiftMod;
        this.amountMod = amountMod;
        this.color = color;
        this.startingAmount = startingAmount;
        this.currentAmount = currentAmount;
    }

    protected FoodItem()
    {
        // NOTHING
    }

}
