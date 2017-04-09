/*
 * Author: Grace Barrett-Snyder
 * Description: Controls the Home UI (Living Room and Yard).
 */

public class PPHomeUIController : PPUIController
{
    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
		if(PlayerPrefsUtil.ShowedFirstHomePrompt)
		{
			promptID = PromptID.ScoutingPrompt;
		}
		else
		{
			promptID = PromptID.FirstHomePrompt;
		}
        base.setReferences();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadHome);
    }

    #endregion

    #region PPUIController Overrides

    protected override void showPopupPrompt()
    {
		if(!PlayerPrefsUtil.ShowedFirstHomePrompt)
		{
			base.showPopupPrompt();
			PlayerPrefsUtil.ShowedFirstHomePrompt = true;
		}	
        else if(!PlayerPrefsUtil.ShowedScoutingPrompt)
        {
            base.showPopupPrompt();
            PlayerPrefsUtil.ShowedScoutingPrompt = true;
        }
    }

    #endregion

}
