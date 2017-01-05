/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended 
{

	protected PPSceneController scene;
    protected PPGameController ppGameController;

	#region MonoBehaviourExtended Overrides

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		scene = PPSceneController.Instance;
        ppGameController = (PPGameController) PPGameController.Instance;
    }

	#endregion

    public void LoadStart() 
	{
		scene.LoadStart();
	}

	public void LoadHome()
	{
		scene.LoadHome();
	}

}
