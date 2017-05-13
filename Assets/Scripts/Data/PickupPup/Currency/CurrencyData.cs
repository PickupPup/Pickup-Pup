/*
 * Author: Grace Barrett-Snyder, Isaiah Mann
 * Description: Holds the data for a specific form of currency (ex: coins)
 */

using System.IO;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public abstract class CurrencyData : ResourceLoader
{
    #region Instance Accessors

    public virtual int Amount
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

	protected PPDataController dataController
	{
		get
		{
			return PPDataController.GetInstance;
		}
	}

    [System.NonSerialized]
    protected SpritesheetDatabase spriteDatabase;

    protected CurrencyType type;
    protected int amount = 0;

    public CurrencyData(CurrencyType type, int initialAmount)
    {
        setup(initialAmount);
        this.type = type;
    }

    protected CurrencyData(int initialAmount)
    {
        setup(initialAmount);
    }

    public virtual void ChangeBy(int deltaAmount)
    {
        amount += deltaAmount;
    }

    public virtual bool CanAfford(int cost)
    {
        return amount > 0 && amount >= cost;
    }
		
	public abstract void Give();

	public CurrencyData GetTakeAmount()
	{
		CurrencyData currency = Copy<CurrencyData>();
		currency.amount *= k.INVERT_VALUE;
		return currency;
	}

    void setup(int initialAmount)
    {
        this.amount = initialAmount;
        checkDatabaseReferences();
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

    public override string ToString()
    {
        return string.Format("{0} {1}", amount, type);
    }

}
