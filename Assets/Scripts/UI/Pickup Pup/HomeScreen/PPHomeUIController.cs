/*
 * 
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the home screen
 */

public class PPHomeUIController : PPUIController 
{
	#region MonoBehaviourExtended Overrides

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		EventController.Event(PPEvent.LoadHome);
    }

	#endregion

}
