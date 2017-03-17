/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class MixpanelAnalyticsInterchange : IAnalyticsInterchange
{
	#region IAnalyticsInterchange Interface

	void IAnalyticsInterchange.SendEvent(AnalyticsEvent targetEvent)
	{
		Mixpanel.SendEvent(targetEvent.ID, targetEvent.Properties);
	}

	#endregion

}
