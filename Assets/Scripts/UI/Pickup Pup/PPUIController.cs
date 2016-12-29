/*
 * Author: Isaiah Mann
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended {
	protected PPSceneController scene;

	protected override void FetchReferences () {
		base.FetchReferences ();
		scene = PPSceneController.Instance;
	}

	public void LoadStart () {
		scene.LoadStart();
	}

	public void LoadHome () {
		scene.LoadHome();
	}
}
