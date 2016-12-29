/*
 * Author: Isaiah Mann
 * Description: Controls the start screen
 */

public class PPStartUIController : PPUIController {
	protected override void FetchReferences () {
		base.FetchReferences ();
		EventController.Event(PPEvent.LoadStart);
	}
}
