/*
 * Authors: Grace Barrett-Snyder, James Hostetler
 * Description: Stores data about the gifts in the game 
 */

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using k = PPGlobal;

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

	Dictionary<GiftEventData, int> activeGiftEventInstances = new Dictionary<GiftEventData, int>();

    #region Database Overrides

    public override void Initialize()
    {
        AssignInstance(this);
        overwriteFromJSONInResources(GIFTS_DIR, this);
    }

    #endregion

	public GiftEventData GetRandomGiftEvent()
	{
		RandomBuffer<GiftEventData> randomGiftEventBuffer = new RandomBuffer<GiftEventData>(giftEvents);
		while(randomGiftEventBuffer.HasNext())
		{
			GiftEventData targetGiftEvent = randomGiftEventBuffer.GetRandom();
			if(canUseEvent(targetGiftEvent))
			{
				return targetGiftEvent;
			}
		}
		return ArrayUtil.GetRandom(this.giftEvents);
	}

	public bool TryUseEvent(GiftEventData activeEvent)
	{
		if(canUseEvent(activeEvent))
		{
			activeGiftEventInstances[activeEvent]++;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void RetireEvent(GiftEventData activeEvent)
	{
		checkActiveEventInstances(activeEvent);
		int activeInstanceCount = activeGiftEventInstances[activeEvent];
		activeInstanceCount = Mathf.Clamp(activeInstanceCount - 1, 0, int.MaxValue);
		activeGiftEventInstances[activeEvent] = activeInstanceCount;
	}

	void checkActiveEventInstances(GiftEventData giftEvent)
	{
		if(!activeGiftEventInstances.ContainsKey(giftEvent))
		{
			activeGiftEventInstances[giftEvent] = k.NONE_VALUE;
		}
	}

	bool canUseEvent(GiftEventData giftEvent)
	{
		checkActiveEventInstances(giftEvent);
		return activeGiftEventInstances[giftEvent] < giftEvent.MaxConcurrentInstances;
	}

}
