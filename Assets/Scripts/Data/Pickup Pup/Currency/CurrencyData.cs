/*
 * Author: Grace Barrett-Snyder, Isaiah Mann
 * Description: Holds the data for a specific form of currency (ex: coins)
 */

using System.IO;
using UnityEngine;

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
            return DefaultSprite;
        }
    }

    public Sprite DefaultSprite
    {
        get
        {
            return Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, DEFAULT));
        }
    }

    #endregion

    protected SpritesheetDatabase spriteDatabase;
    protected CurrencyType type;
    protected int amount = 0;

    public CurrencyData(CurrencyType type, int initialAmount)
    {
        spriteDatabase = SpritesheetDatabase.GetInstance;

        this.type = type;
        amount = initialAmount;
    }

    protected CurrencyData(int initialAmount)
    {
        spriteDatabase = SpritesheetDatabase.GetInstance;
    }

    public virtual void IncreaseBy(int deltaAmount)
    {
        amount += deltaAmount;
    }

    public virtual bool CanAfford(int cost)
    {
        return amount >= cost;
    }

	void checkDatabaseReferences()
	{
		if(spriteDatabase == null)
		{
			spriteDatabase = SpritesheetDatabase.GetInstance;
		}
	}

	protected Sprite fetchSprite(string spriteName)
	{
		checkDatabaseReferences();
		Sprite icon;
		spriteDatabase.TryGetSprite(spriteName, out icon);
		return icon;
	}

}
