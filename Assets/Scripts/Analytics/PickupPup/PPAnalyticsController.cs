/*
 * Author(s): Isaiah Mann
 * Description: Handles Analytics for Pickup Pup
 * Usage: [no notes]
 */

public class PPAnalyticsController : AnalyticsController
{
	#region Static Accessors

	public static PPAnalyticsController GetInstance
	{
		get
		{
			return Instance as PPAnalyticsController;
		}
	}

	#endregion

	#region AnalayticsController Overrides

	// Should be modified if we opt to use a different service than mixpanel
	protected override IAnalyticsInterchange getInterchange()
	{
		return new MixpanelAnalyticsInterchange();
	}

	#endregion

}
