/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the yard screen
 */

public class PPYardUIController : PPUIController
{
    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        promptID = PromptID.ScoutingPrompt;
        base.setReferences();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadYard);
    }

    #endregion

    public void OnDoorTap()
    {
        LoadLivingRoom();
    }

}
