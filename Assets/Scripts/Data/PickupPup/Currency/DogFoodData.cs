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
    public DogFoodData(int initialAmount, DogFoodType dogFoodType) : base(initialAmount)
    {
        //Read in amount from JSON for int corresponding to dogFoodType (0,1,2)
        float specialGiftRate;
        int amountGiftRate;
        DogFoodType foodType = dogFoodType;
        switch ((int)foodType)
        {
            case 0:
                specialGiftRate = .1f;
                amountGiftRate = 1;
                break;
            case 1:
                specialGiftRate = .1f;
                amountGiftRate = 2;
                break;
        }
        amount = initialAmount;
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
