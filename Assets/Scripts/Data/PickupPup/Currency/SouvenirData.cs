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

    #endregion

    [SerializeField]
    string name;
    [SerializeField]
    string description;

    DogDescriptor dog;

}
