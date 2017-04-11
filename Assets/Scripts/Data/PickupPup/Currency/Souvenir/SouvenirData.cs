/*
 * Author(s): Isaiah Mann
 * Description: Describes a souvenir
 * Usage: [no notes]
 */

using UnityEngine;

[System.Serializable]
public class SouvenirData : CurrencyData
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

    #endregion

    [SerializeField]
    string name;
    [SerializeField]
    string displayName;
    [SerializeField]
    string description;

    DogDescriptor dog;

    public SouvenirData(int amount = 1) : base(CurrencyType.Souvenir, amount){}

    public static SouvenirData Default()
    {
        SouvenirData defaultSouvenir = new SouvenirData();
        defaultSouvenir.name = string.Empty;
        defaultSouvenir.description = string.Empty;
        defaultSouvenir.dog = null;
        return defaultSouvenir;
    }

}
