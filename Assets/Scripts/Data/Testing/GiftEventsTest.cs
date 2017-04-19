/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class GiftEventsTest : MonoBehaviourExtended 
{	
    GiftDatabase gifts;

	protected override void setReferences() 
	{
		base.setReferences();
        gifts = GiftDatabase.GetInstance;
        UnityEngine.Debug.Log(gifts.GiftEvents[0].EventDescription);
	}

}
