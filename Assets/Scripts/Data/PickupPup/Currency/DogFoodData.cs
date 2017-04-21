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
    
    public FoodDatabase Foods
    {
        get
        {
            return foods;
        }
    }

    public FoodItem[] FoodItems
    {
        get
        {
            return foodItems;
        }
    }
    
    FoodDatabase foods;
    FoodItem[] foodItems;

    public DogFoodData() : base()
    {
        //Grab all current food types and respective effects & starting amounts.
        foods = FoodDatabase.Instance;
        foodItems = foods.Items;
    }

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
