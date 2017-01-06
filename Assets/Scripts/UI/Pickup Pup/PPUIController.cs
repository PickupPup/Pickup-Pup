/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended 
{
	protected PPSceneController sceneController;
    protected PPGameController gameController;

	#region MonoBehaviourExtended Overrides

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		sceneController = PPSceneController.Instance;
		gameController = PPGameController.GetInstance;
    }

	#endregion

    public void LoadStart() 
	{
		sceneController.LoadStart();
	}

	public void LoadHome()
	{
		sceneController.LoadHome();
	}

}
