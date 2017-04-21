/*
 * Authors: Grace Barrett-Snyder, Ben Page
 * Description: Stores data about the food brands in the game 
 */

using UnityEngine;
using System.IO;

[System.Serializable]
public class FoodDatabase : Database<FoodDatabase>
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

    public FoodItem[] Food
    {
        get
        {
            return food;
        }
    }

    #endregion

    [SerializeField]
    FoodItem[] food;

    #region Database Overrides

    public override void Initialize()
    {
        AssignInstance(this);
    }

    #endregion

}
