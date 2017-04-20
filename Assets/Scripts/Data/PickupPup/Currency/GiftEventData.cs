/*
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

    public string EventName
    {
        get
        {
            return eventName;
        }
    }

    public string EventDescription
    {
        get
        {
            return eventDescription;
        }
    }

    public string EventSprite
    {
        get
        {
            return eventSprite;
        }
    }

    public string[] BonusTypes
    {
        get
        {
            return bonusTypes;
        }
    }

    public int[] BonusAmounts
    {
        get
        {
            return bonusAmounts;
        }
    }

    public float[] BonusChances
    {
        get
        {
            return bonusChances;
        }
    }

    #endregion

    [SerializeField]
    string eventName;
    [SerializeField]
    string eventDescription;
    [SerializeField]
    string eventSprite;
    [SerializeField]
    string[] bonusTypes;
    [SerializeField]
    int[] bonusAmounts;
    [SerializeField]
    float[] bonusChances;

    public GiftEventData(int amount = 1) : base(CurrencyType.GiftEvent, amount){}

    public CurrencyData[] GetCurrencies()
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

    public void Call()
    {
        CurrencyData[] data = GetCurrencies();
        PPDataController controller = PPDataController.GetInstance;
        foreach(CurrencyData currency in data)
        {
            controller.ChangeCurrencyAmount(currency.Type, currency.Amount);
        }
    }

}
