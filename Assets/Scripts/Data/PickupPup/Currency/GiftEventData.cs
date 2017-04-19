/*
 * Author(s): Isaiah Mann
 * Description: A special kind of dog gift with a narrative event tied to it
 * Usage: [no notes]
 */

using System;

using UnityEngine;

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

    public float[] BonusChanges
    {
        get
        {
            return bonusChanges;
        }
    }

    #endregion

    [SerializeField]
    string eventName;
    [SerializeField]
    string eventDescription;
    [SerializeField]
    string[] bonusTypes;
    [SerializeField]
    int[] bonusAmounts;
    [SerializeField]
    float[] bonusChanges;

    public GiftEventData(int amount = 1) : base(CurrencyType.GiftEvent, amount){}

    public CurrencyData[] GetCurrencies()
    {
        int numCurrencies = bonusTypes.Length;
        CurrencyFactory factory = new CurrencyFactory();
        CurrencyData[] currencies = new CurrencyData[numCurrencies];
        for(int i = 0; i < numCurrencies; i++)
        {
            try 
            {
                CurrencyType type = (CurrencyType) Enum.Parse(typeof(CurrencyType), bonusTypes[i]);
            }
            catch
            {
                Debug.LogErrorFormat("Unable to parse currency {0}", bonusTypes[i]);
                // Skip this gift type:
                continue;
            }
        }
        return currencies;
    }

}
