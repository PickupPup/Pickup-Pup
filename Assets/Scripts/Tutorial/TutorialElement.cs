/*
 * Author(s): Isaiah Mann
 * Description: Describes a single element in a tutorial
 * Usage: [no notes]
 */

using System.Collections.Generic;

using UnityEngine;

public class TutorialElement : TutorialObject
{
    static Dictionary<string, TutorialElement> elementLookup = new Dictionary<string, TutorialElement>();

    string lookupKey
    {
        get
        {
            if(string.IsNullOrEmpty(accessorKeyOverride))
            {
                return gameObject.name;
            }
            else
            {
                return accessorKeyOverride;
            }
        }
    }

    [Tooltip("Leave blank if you want to just use the GameObject's name as the key")]
    [SerializeField]
    string accessorKeyOverride = string.Empty;
    [SerializeField]
    bool overwriteKey = false;

    public static bool Fetch(string key, out TutorialElement element)
    {
        return elementLookup.TryGetValue(key, out element);
    }

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        if(!tryTrackingInLookup())
        {
            Debug.LogErrorFormat("{0} not added to lookup. Key already exists", lookupKey);
        }
    }

    #endregion

    bool tryTrackingInLookup()
    {
        string key = this.lookupKey;
        if(elementLookup.ContainsKey(key))
        {
            if(overwriteKey)
            {
                elementLookup[key] = this;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            elementLookup.Add(key, this);
            return true;
        }
    }

}
