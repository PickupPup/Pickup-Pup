/*
 * Author(s): Isaiah Mann
 * Description: Analytics Event for Pickup Pup
 * Usage: [no notes]
 */

public abstract class PPAnalyticsEvent : AnalyticsEvent
{ 
    public PPAnalyticsEvent(string id, string[] propertyKeys, object[] propertyVals) :
    base(id, propertyKeys, propertyVals)
    {
        // NOTHING
    }

}
