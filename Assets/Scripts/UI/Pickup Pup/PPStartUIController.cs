/*
 * Author: Isaiah Mann
 * Description: Controls the start screen
 */

public class PPStartUIController : PPUIController 
{
	#region MonoBehaviourExtended Overrides

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		EventController.Event(PPEvent.LoadStart);
	}

	#endregion

}
