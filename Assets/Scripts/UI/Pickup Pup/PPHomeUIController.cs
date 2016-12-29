/*
 * Author: Isaiah Mann
 * Description: Controls the home screen
 */

public class PPHomeUIController : PPUIController {
	protected override void FetchReferences () {
		base.FetchReferences ();
		EventController.Event(PPEvent.LoadHome);
	}
}
