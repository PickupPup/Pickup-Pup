/*
 * Author: Grace Barrett-Snyder, Ben Page
 * Description: Stores data for the Player's dog food currency.
 */

using System.IO;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class DogFoodData : CurrencyData
{
    #region Instance Accessors

    public string FoodType
    {
        get
        {
            return foodType;
        }
    }

    public float SpecialGiftMod
    {
        get
        {
            return specialGiftMod;
        }
    }

    public float AmountMod
    {
        get
        {
            return amountMod;
        }
    }

    public string Color
    {
        get
        {
            return color;
        }
    }

    public string ColorHex
    {
        get
        {
            return colorHex;
        }
    }

    // Translates Hexadecimal color into Unity Color:
    public Color GameColor
    {
        get
        {
            return parseHexColor(this.ColorHex);
        }
    }

    #endregion

    FoodDatabase foods
    {
        get
        {
            return FoodDatabase.Instance;
        }
    }

    [SerializeField]
    string foodType;
    [SerializeField]
    float specialGiftMod;
    [SerializeField]
    float amountMod;
    [SerializeField]
    string color;
    [SerializeField]
    string colorHex;

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
            if (type == CurrencyType.DogFood)
            {
                return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DOG_FOOD_ICON));
            }
            else
            {
                /* BP Once there is a special icon sprite to load from Resources, load it via the commented
                 * out line below. For now, load the regular icon.
                 */
                return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DOG_FOOD_ICON));
                //return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DOG_FOOD_SPECIAL_ICON));
            }
        }
    }

    #endregion

}
