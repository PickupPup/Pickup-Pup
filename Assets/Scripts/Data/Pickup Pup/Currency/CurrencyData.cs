/*
 * Author: Grace Barrett-Snyder, Isaiah Mann
 * Description: Holds the data for a specific form of currency (ex: coins)
 */

using System.IO;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class CurrencyData : ResourceLoader
{
    #region Instance Accessors

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public CurrencyType Type
    {
        get
        {
            return type;
        }
    }

    public virtual Sprite Icon
    {
        get
        {
            return Resources.Load<Sprite>(Path.Combine(k.SPRITES_DIR, k.DEFAULT));
        }
    }

    #endregion

    protected CurrencyType type;
    protected int amount = 0;

    public CurrencyData(CurrencyType type, int initialAmount)
    {
        this.type = type;
        amount = initialAmount;
    }

    protected CurrencyData(int initialAmount)
    {
        // NOTHING
    }

    public virtual void IncreaseBy(int deltaAmount)
    {
        amount += deltaAmount;
    }

    public virtual bool CanAfford(int cost)
    {
        return amount >= cost;
    }

}
