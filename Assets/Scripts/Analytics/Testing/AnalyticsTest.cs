/*
 * Author(s): Isaiah Mann
 * Description: Verifies that anayltics are working in the game
 * Usage: [no notes]
 */

public class AnalyticsTest : MonoTest 
{	
	#region MonoTest Overrides 

	public override bool RunTest(out string feedback)
	{
		feedback = string.Empty;
//		try
//		{
			// Assume we're using mixpanel
			Mixpanel.ToggleSendInEditor(true);
			analytics.SendEvent(new AnalyticsEvent(
				"Test Event", 
				new string[]{"Message"},
				new string[]{"Hello World"}));
			Mixpanel.ToggleSendInEditor(false);
			return true;
//		}
//		catch(System.Exception e)
//		{
//			UnityEngine.Debug.LogError(e);
//			return false;
//		}
	}

	#endregion
}
