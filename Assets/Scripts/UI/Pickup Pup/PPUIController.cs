/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended {

	protected PPSceneController scene;
    protected PPGameController ppGameController;

    protected override void FetchReferences () {
		base.FetchReferences ();
		scene = PPSceneController.Instance;
        ppGameController = (PPGameController) PPGameController.Instance;
    }

    public void LoadStart () {
		scene.LoadStart();
	}

	public void LoadHome () {
		scene.LoadHome();
	}
}