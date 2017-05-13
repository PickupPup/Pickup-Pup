/*
 * Author: Grace Barrett-Snyder
 * Description: Describes an item in the shop
 */

using UnityEngine;
using System;
using k = PPGlobal;

[Serializable]
public class ShopItem : PPData
{
    #region Static Accessors

    public static ShopItem Default
    {
        get
        {
            ShopItem shopItem = new ShopItem();
            shopItem.itemName = string.Empty;
            return shopItem;
        }
    }

    #endregion

    #region Instance Accessors

	public CurrencyData Cost
	{
		get
		{
			return new CurrencyFactory().Create(CostCurrencyType, CostAmount);
		}
	}

	public CurrencyData Value
	{
		get
		{
			return currency;
		}
	}

    public int CostAmount
    {
        get
        {
            return cost;
        }
    }

    public string CostStr
    {
        get
        {
            return PPData.FormatCost(cost);
        }
    }

    public string ItemName
    {
        get
        {
            return itemName;
        }
    }

    public CurrencyType CostCurrencyType
    {
        get
        {
			return (CurrencyType) Enum.Parse(typeof(CurrencyType), costCurrencyType);
        }
    }

    public CurrencyType ValueCurrencyType
    {
        get
        {
			return (CurrencyType) Enum.Parse(typeof(CurrencyType), valueCurrencyType);
        }
    }
		
    public Sprite Icon
    {
        get
        {
			if(currency == null)
			{
	            if(_icon == null)
	            {
	                if(!spritesheetDatabase.TryGetSprite(icon, out _icon))
	                {
	                    _icon = DogDatabase.DefaultSprite;
	                }
	            }
	            return _icon;
			}
			else
			{
				return currency.Icon;
			}
        }
    }

	public Color IconColor
	{
		get
		{
			if(currency == null)
			{
				return Color.white;
			}
			else
			{
				if(ValueCurrencyType == CurrencyType.DogFood)
				{
					return (currency as DogFoodData).Color;
				}
				else
				{
					return Color.white;
				}
			}
		}
	}

    #endregion

    SpritesheetDatabase spritesheetDatabase
    {
        get
        {
            if(_spritesheetDatabase == null)
            {
                _spritesheetDatabase = SpritesheetDatabase.GetInstance;
            }
            return _spritesheetDatabase;
        }
    }

	CurrencyData currency;

    [SerializeField]
    int cost;
    [SerializeField]
    string itemName;
    [SerializeField]
	string costCurrencyType;
    [SerializeField]
	string valueCurrencyType;
    [SerializeField]
    string icon;
	[SerializeField]
	string valueCurrencySubtype;

    Sprite _icon;
    SpritesheetDatabase _spritesheetDatabase;

    public ShopItem(
        string itemName,
		string valueCurrencyType,
        int cost, 
		string costCurrencyType
        )
    {
        this.itemName = itemName;
        this.valueCurrencyType = valueCurrencyType;
        this.cost = cost;
        this.costCurrencyType = costCurrencyType;       
    }

    protected ShopItem()
    {
        // NOTHING
    }

	public void SetCurrency()
	{
		if(ValueCurrencyType == CurrencyType.DogFood)
		{
			currency = FoodDatabase.GetInstance.Get(valueCurrencySubtype).Copy();
			int difference = k.SINGLE_VALUE - currency.Amount;
			currency.ChangeBy(difference);
		}
		else
		{
			throw new System.NotImplementedException();
		}
	}

	public int GetTotalCost(int amount)
	{
		return this.CostAmount * amount;
	}

}
