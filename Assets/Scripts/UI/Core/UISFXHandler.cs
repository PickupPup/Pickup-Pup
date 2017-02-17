/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: For playing SFX on events
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.EventSystems;
using k = PPGlobal;

public class UISFXHandler : UIInteractable, IPointerClickHandler
{
    [SerializeField]
    string clickEnabledSoundEvent = k.GetPlayEvent(k.MENU_CLICK);
    [SerializeField]
    string clickDisabledSoundEvent = k.GetPlayEvent(k.EMPTY);

    #region IPointerClickHandler Interface

    void IPointerClickHandler.OnPointerClick(PointerEventData ptrEvent)
    {
        if(interactable)
        {
            EventController.Event(clickEnabledSoundEvent);
        }
        else
        {
            EventController.Event(clickDisabledSoundEvent);
        }
    }

    #endregion

    public void SetInteractable(bool isInteractable)
    {
        interactable = isInteractable;
    }

}
