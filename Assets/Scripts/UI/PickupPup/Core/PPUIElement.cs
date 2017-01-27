/*
 * Author(s): Isaiah Mann
 * Description: UI Elements for Pickup Pup
 */

public class PPUIElement : UIElement 
{
    protected PPGameController gameController;
    protected PPSceneController sceneController;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        gameController = PPGameController.GetInstance;
        sceneController = PPSceneController.Instance;
    }

    protected virtual bool requestReloadScene()
    {
        return sceneController.RequestReloadCurrentScene();
    }

    #endregion
}
