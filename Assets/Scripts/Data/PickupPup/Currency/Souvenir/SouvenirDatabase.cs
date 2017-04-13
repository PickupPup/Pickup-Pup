/*
 * Author(s): Isaiah Mann
 * Description: Stores all the information for souvenirs
 * Usage: [no notes]
 */

using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

using k = PPGlobal;

public class SouvenirDatabase : Database<SouvenirDatabase>
{
    #region Static Accessors

    public static Sprite DefaultSprite
    {
        get 
        {
            if(_defaultSprite)
            {
                return _defaultSprite;
            } 
            else 
            {
                // Memoization for efficiency
                _defaultSprite = Resources.LoadAll<Sprite>(Path.Combine(SPRITES_DIR, k.UI_SPRITESHEET))[0];
                return _defaultSprite;
            }
        }
    }

    #endregion

    static Sprite _defaultSprite;

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
    SpritesheetDatabase sprites;

    public override void Initialize()
    {
        base.Initialize();
        overwriteFromJSONInResources(SOUVENIRS, this);
        this.souvenirLookup = generateSouvenirLookup(souvenirs);
        this.sprites = SpritesheetDatabase.GetInstance;
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

    public Sprite GetSprite(SouvenirData souvenir)
    {
        Sprite souvenirSprite;
        if(sprites.TryGetSprite(getSpriteName(souvenir), out souvenirSprite))
        {
            return souvenirSprite;
        }
        else 
        {
            return DefaultSprite;
        }
    }

    string getSpriteName(SouvenirData souvenir)
    {
        return string.Format("{0}{1}{2}", k.SOUVENIRS, k.JOIN_CHAR, souvenir.Name);
    }

}
