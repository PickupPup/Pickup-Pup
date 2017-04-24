/*
 * Author(s): Isaiah Mann
 * Description: Describes a souvenir
 * Usage: [no notes]
 */

using UnityEngine;

using System;

[System.Serializable]
public class SouvenirData : SpecialGiftData
{
    #region Instance Accessors

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public string Description
    {
        get
        {
            return this.description;
        }
    }

    public string DisplayName
    {
        get
        {
            return this.displayName;
        }
    }

    public override Sprite Icon 
    {
        get 
        {
            return database.GetSprite(this);
        }
    }

    public DogDescriptor Owner
    {
        get
        {
            return this.dog;
        }
    }


    SouvenirDatabase database
    {
        get
        {
            return SouvenirDatabase.GetInstance;
        }
    }

    public bool IsCollected
    {
        get
        {
            return this.isCollected;
        }
    }
     
    public DateTime DateCollected 
    {
        get;
        private set;
    }

    #endregion

    [SerializeField]
    string name;
    [SerializeField]
    string displayName;
    [SerializeField]
    string description;

    DogDescriptor dog;
    bool isCollected = false;

    public SouvenirData(int amount = 1) : base(CurrencyType.Souvenir, amount){}

    public static SouvenirData Default()
    {
        SouvenirData defaultSouvenir = new SouvenirData();
        defaultSouvenir.name = string.Empty;
        defaultSouvenir.description = string.Empty;
        defaultSouvenir.dog = null;
        defaultSouvenir.isCollected = false;
        defaultSouvenir.DateCollected = default(DateTime);
        return defaultSouvenir;
    }

    public void SetOwner(DogDescriptor dog)
    {
        this.dog = dog;
    }

    public void Collect()
    {
        this.toggleUnlocked(true);
        this.DateCollected = DateTime.Now;
    }

    public void Lock()
    {
        this.toggleUnlocked(false);
        this.DateCollected = default(DateTime);
    }

    public SouvenirData Copy()
    {
        return this.Copy<SouvenirData>();
    }

    void toggleUnlocked(bool isUnlocked)
    {
        this.isCollected = isUnlocked;
    }

	#region CurrencyData Overrides

	public override void Give()
	{
		Collect();
	}

	#endregion

    #region Object Overrides

    public override string ToString ()
    {
        return this.DisplayName;
    }

    #endregion

}
