/*
 * Authors: Grace Barrett-Snyder, James Hostetler
 * Description: Stores data about the gifts in the game 
 */

using UnityEngine;
using System.IO;

[System.Serializable]
public class GiftDatabase : Database<GiftDatabase>
{
    static Sprite defaultSprite
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
                _defaultSprite = Resources.Load<Sprite>(Path.Combine(SPRITES_DIR, GIFTS_DIR));
                return _defaultSprite;
            }
        }
    }

    static Sprite _defaultSprite;

    #region Instance Accessors

    public GiftItem[] Gifts
    {
        get
        {
            return gifts;
        }
    }

    #endregion

    [SerializeField]
    GiftItem[] gifts;

    #region Database Overrides

    public override void Initialize()
    {
        AssignInstance(this);
    }

    #endregion

}
