/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class UIInteractable : UIElement
{
    protected UISFXHandler sfxHandler;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences ();
        sfxHandler = ensureReference<UISFXHandler>();
    }

    #endregion

}
