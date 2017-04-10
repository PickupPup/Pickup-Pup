/*
 * Author(s): Isaiah Mann
 * Description: Stores all the information for souvenirs
 * Usage: [no notes]
 */

using UnityEngine;

public class SouvenirDatabase : Database<SouvenirDatabase>
{
    #region Instance Acessors 

    public SouvenirData[] Souvenirs
    {
        get
        {
            return this.souvenirs;
        }
    }

    #endregion

    [SerializeField]
    SouvenirData[] souvenirs;

    public override void Initialize()
    {
        base.Initialize();
        overwriteFromJSONInResources(SOUVENIRS, this);
    }

}
