/*
 * Author: Grace Barrett-Snyder 
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

	public Color Color
	{
		get
		{
			if(!colorIsSet)
			{
				ColorUtility.TryParseHtmlString(colorHex, out _color);
			}
			return _color;
		}
	}

	public string ColorStr
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

	#endregion

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

	Color _color = Color.white;
	bool colorIsSet = false;

    public DogFoodData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.DogFood;
        amount = initialAmount;
    }

    #region CurrencyData Overrides

    // TODO: Finish this when the currency icons are imported
    public override Sprite Icon
    {
        get
        {
			return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DOG_FOOD_ICON));
        }
    }

	public override void Give()
	{
		dataController.ChangeFood(this.Amount);
	}

    #endregion

	public DogFoodData Copy()
	{
		return Copy<DogFoodData>();
	}

}
