﻿/*
 * Author(s): Isaiah Mann
 * Description: A special kind of dog gift with a narrative event tied to it
 * Usage: [no notes]
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class GiftEventData : SpecialGiftData
{
	#region Instance Accessors

	#region CurrencyData Overrides

	public override Sprite Icon 
	{
		get
		{
			Sprite eventIcon;
			if(SpritesheetDatabase.GetInstance.TryGetSprite(eventSprite, out eventIcon))
			{
				return eventIcon;
			}
			else
			{
				return base.Icon;
			}
		}
	}

	#endregion

	public int MaxConcurrentInstances
	{
		get
		{
			return maxConcurrentInstances;
		}
	}

	#endregion

	CurrencyData[] result
	{
		get
		{
			if(_result == null)
			{
				_result = calculateResult();
			}
			return _result;
		}
	}

	CurrencyData[] _result;

	[SerializeField]
	string eventName;
	[SerializeField]
	string eventDescription;
	[SerializeField]
	string eventFailDescription;
	[SerializeField]
	string eventSprite;
	[SerializeField]
	int maxConcurrentInstances;
	[SerializeField]
	string[] bonusTypes;
	[SerializeField]
	int[] bonusAmounts;
	[SerializeField]
	float[] bonusChances;

	// Default constructor which can be called w/out arguments
    public GiftEventData(int amount = 1) : base(CurrencyType.GiftEvent, amount)
	{
		// NOTHING
	}

	public CurrencyData[] GetResult()
	{
		return this.result;
	}

    CurrencyData[] calculateResult()
    {
        int numCurrencies = bonusTypes.Length;
        CurrencyFactory factory = new CurrencyFactory();
        List<CurrencyData> currencies = new List<CurrencyData>();
        for(int i = 0; i < numCurrencies; i++)
        {
            // Random roll to determine whether this percent is included:
            if(UnityEngine.Random.Range(k.NONE_VALUE, k.FULL_PERCENT_DECIMAL) <= bonusChances[i])
            {
                currencies.Add(factory.Create(bonusTypes[i], bonusAmounts[i]));
            }
        }
        return currencies.ToArray();
    }

	#region CurrencyData Overrides

    public override void Give()
	{
		if(checkEventSuccess(this.result))
		{
			foreach(CurrencyData currency in this.result)
	        {
				dataController.ChangeCurrencyAmount(currency);
	        }
			EventController.Event(eventName);
		}
		GiftDatabase.GetInstance.RetireEvent(this);
    }

	#endregion

	bool checkEventSuccess(CurrencyData[] currencyChanges)
	{
		foreach(CurrencyData currency in currencyChanges)
		{
			// If this currency constitutes a deduction:
			if(currency.Amount < k.NONE_VALUE && !dataController.CanAfford(currency.Type, Mathf.Abs(currency.Amount)))
			{
				return false;
			}
		}
		// Fall through case (can afford all deductions):
		return true;
	}

	public override string ToString ()
	{
		if(checkEventSuccess(this.result))
		{
			return this.eventDescription;
		}
		else
		{
			return this.eventFailDescription;
		}
	}

}
