/*
 * Author(s): Isaiah Mann
 * Description: Stores all the information for souvenirs
 * Usage: [no notes]
 */

using UnityEngine;

using System;
using System.Collections.Generic;

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
    Dictionary<string, SouvenirData> souvenirLookup;

    public override void Initialize()
    {
        base.Initialize();
        overwriteFromJSONInResources(SOUVENIRS, this);
        this.souvenirLookup = generateSouvenirLookup(souvenirs);
    }

    public SouvenirData Get(string souvenirName)
    {
        SouvenirData data;
        if(!souvenirLookup.TryGetValue(souvenirName, out data))
        {
            data = SouvenirData.Default();
        }
        return data;
    }

    Dictionary<string, SouvenirData> generateSouvenirLookup(SouvenirData[] list)
    {
        Dictionary<string, SouvenirData> lookup = new Dictionary<string, SouvenirData>();
        foreach(SouvenirData souvenir in list)
        {
            try
            {
                lookup.Add(souvenir.Name, souvenir);
            }
            catch(ArgumentException error)
            {
                Debug.LogErrorFormat
                (
                    "Souvenir with name {0} already exists. Stacktrace:\n{1}", 
                    souvenir.Name, error
                );
            }
        }
        return lookup;
    }

}
