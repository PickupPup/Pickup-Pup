/*
 * Author(s): Isaiah Mann
 * Description: UI Elements for Pickup Pup
 */

public class PPUIElement : UIElement 
{
    protected PPGameController game;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        game = PPGameController.GetInstance;
    }

    #endregion
}
