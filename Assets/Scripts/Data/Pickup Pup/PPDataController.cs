/*
 * Author: Isaiah Mann
 * Desc: Handles save for Pickup Pup
 */

public class PPDataController : DataController {

	protected override SerializableData getDefaultFile () {
		return new PPGameSave();
	}		
}
