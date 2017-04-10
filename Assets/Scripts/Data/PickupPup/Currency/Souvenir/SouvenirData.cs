/*
 * Author(s): Isaiah Mann
 * Description: Describes a souvenir
 * Usage: [no notes]
 */

using UnityEngine;

[System.Serializable]
public class SouvenirData 
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

    public DogDescriptor Owner
    {
        get
        {
            return this.dog;
        }
    }

    #endregion

    [SerializeField]
    string name;
    [SerializeField]
    string description;

    DogDescriptor dog;

    public static SouvenirData Default()
    {
        SouvenirData defaultSouvenir = new SouvenirData();
        defaultSouvenir.name = string.Empty;
        defaultSouvenir.description = string.Empty;
        defaultSouvenir.dog = null;
        return defaultSouvenir;
    }

}
