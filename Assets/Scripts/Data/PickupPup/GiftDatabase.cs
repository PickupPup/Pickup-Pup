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

    public GiftEventData[] GiftEvents
    {
        get
        {
            return giftEvents;
        }
    }

    #endregion

    [SerializeField]
    GiftEventData[] giftEvents;

    #region Database Overrides

    public override void Initialize()
    {
        AssignInstance(this);
        overwriteFromJSONInResources(GIFTS_DIR, this);
    }

    #endregion

	public GiftEventData GetRandomGiftEvent()
	{
		return ArrayUtil.GetRandom(this.GiftEvents);
	}

}
