/*
 * Author(s): Isaiah Mann
 * Description: Event describing a single play session
 * Usage: [no notes]
 */

using UnityEngine;
using k = PPGlobal;

public class PlaySessionEvent : PPAnalyticsEvent
{
    const string PLAY_SESSION = "Play Session";

    public PlaySessionEvent(PPDataController data) : 
    base(PLAY_SESSION, getPropertyKeys(), getPropertyValues(data))
    {
        // NOTHING
    }

    static string[] getPropertyKeys()
    {
        return new string[]{k.PLAY_TIME, k.FEED_COUNT};
    }

    static object[] getPropertyValues(PPDataController data)
    {
        return new object[]{string.Format("{0} {1}", Time.time, k.SECONDS), data.TimesDogFoodRefilled};
    }

}
