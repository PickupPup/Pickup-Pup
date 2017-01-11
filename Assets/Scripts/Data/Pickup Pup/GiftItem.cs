/*
 * Author: Grace Barrett-Snyder, James Hostetler
 * Description: Stores gift information.
 */

using UnityEngine;
using System;

[Serializable]
public class GiftItem : PPData 
{
    #region Static Accessors

    public static GiftItem Default
    {
        get
        {
            GiftItem giftItem = new GiftItem();
            giftItem.giftName = string.Empty;
            return giftItem;
        }
    }

    #endregion

	#region Instance Accessors

	public string GiftName
    {
        get
        {
            return giftName;
        }
    }


    public int Value
    {
        get
        {
            return value;
        }
    }

    public string ValueStr
    {
        get
        {
            return formatCost(value);
        }
    }


    public CurrencyType ValueCurrencyType
    {
        get
        {
            return valueCurrencyType;
        }
    }


    public Dog GiftDog
    {
        get
        {
            return giftDog;
        }
    }

    #endregion

    [SerializeField]
    int value;
    [SerializeField]
    string giftName;
    [SerializeField]
    CurrencyType valueCurrencyType;

	// TEMP
	[SerializeField]
    Dog giftDog;


	public GiftItem(
        string giftName,
        int value,
        CurrencyType valueCurrencyType
        )
    {
        this.giftName = giftName;
        this.value = value;
        this.valueCurrencyType = valueCurrencyType; 
    }

	protected GiftItem()
    {
        // NOTHING
    }

}
