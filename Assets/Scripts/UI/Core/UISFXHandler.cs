/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: For playing SFX on events
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using k = PPGlobal;

public class UISFXHandler : UIInteractable, IPointerDownHandler
{
    [SerializeField]
    string clickEnabledSoundEvent = k.GetPlayEvent(k.MENU_CLICK);
    [SerializeField]
    string clickDisabledSoundEvent = k.GetPlayEvent(k.EMPTY);

    Button button;

    bool isActive = true;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        button = GetComponent<Button>();
    }

    #endregion

    #region IPointerDownHandler Interface

    void IPointerDownHandler.OnPointerDown(PointerEventData ptrEvent)
    {
        if(isActive)
        {
            if(button && button.enabled && button.interactable)
            {
                EventController.Event(clickEnabledSoundEvent);
            }
            else
            {
                EventController.Event(clickDisabledSoundEvent);
            }
        }
    }

    #endregion

    public void ToggleActive(bool isActive)
    {
        this.isActive = isActive;
    }

}
