/*
 * Author(s): Isaiah Mann
 * Description: UI Elements for Pickup Pup
 */

using k = PPGlobal;

public class PPUIElement : UIElement 
{
    protected const string FIND_GIFT = k.FIND_GIFT;
    protected const string REDEEM_GIFT = k.REDEEM_GIFT;
    protected const string TAP_TO_REDEEM = k.TAP_TO_REDEEM;

    protected PPGameController gameController;
    protected PPSceneController sceneController;
    protected LanguageDatabase languageDatabase;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        gameController = PPGameController.GetInstance;
        sceneController = PPSceneController.Instance;
        languageDatabase = LanguageDatabase.Instance;
    }

    protected virtual bool requestReloadScene()
    {
        return sceneController.RequestReloadCurrentScene();
    }

    #endregion
}
