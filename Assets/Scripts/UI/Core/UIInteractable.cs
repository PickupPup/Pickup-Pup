/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class UIInteractable : UIElement
{
    protected UISFXHandler sfxHandler;
    protected bool interactable;

    protected override void setReferences()
    {
        base.setReferences ();
        sfxHandler = ensureReference<UISFXHandler>();
    }

}
