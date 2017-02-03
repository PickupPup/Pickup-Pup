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
            return PPData.FormatCost(value);
        }
    }

    public CurrencyType ValueType
    {
 		get 
		{
			return (CurrencyType) Enum.Parse(typeof(CurrencyType), valueType); 
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
    string giftName;
    [SerializeField]
    int value;
    [SerializeField]
    string valueType;

	// TEMP
	[SerializeField]
    Dog giftDog;

	public GiftItem(
        string giftName,
        int value,
        string valueType
        )
    {
        this.giftName = giftName;
        this.value = value;
        this.valueType = valueType; 
    }

	protected GiftItem()
    {
        // NOTHING
    }

}
