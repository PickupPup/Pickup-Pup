/*
 * Author(s): Isaiah Mann
 * Description: Dog related event
 * Usage: [no notes]
 */

using k = PPGlobal;

public class DogAnalyticsEvent : PPAnalyticsEvent 
{
    public const string DOG_ADOPTED = "Dog Adopted";
    public const string DOG_SEND_TO_SCOUT = "Dog Sent to Scout";
    public const string GIFT_REDEEMED = "Gift Redeemed";

    public DogAnalyticsEvent(string id, DogDescriptor dog, bool includeScoutingInfo = false) :
    base(id, getPropertyKeys(includeScoutingInfo), getPropertyValues(dog, includeScoutingInfo)) 
    {
        // NOTHING
    }

    static string[] getPropertyKeys(bool includeScoutingInfo = false)
    {
        if(includeScoutingInfo)
        {
            return new string[]{k.NAME, k.DOG_TIMER_LENGTH};
        }
        else
        {
            return new string[]{k.NAME};
        }
    }
       
    static object[] getPropertyValues(DogDescriptor dog, bool includeScoutingInfo = false)
    {
        if(includeScoutingInfo)
        {
            return new object[]{dog.Name, string.Format("{0} {1}", dog.TotalTimeToReturn, k.SECONDS)};
        }
        else
        {
            return new object[]{dog.Name};
        }
    }

}
