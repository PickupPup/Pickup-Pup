/*
 * Author(s): Isaiah Mann
 * Description: For playing SFX on events
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.EventSystems;
using k = PPGlobal;

public class UISFXHandler : UIElement, IPointerClickHandler
{
    [SerializeField]
    string clickSoundEvent = k.GetPlayEvent(k.MENU_CLICK);

    #region IPointerClickHanlder Interface

    void IPointerClickHandler.OnPointerClick(PointerEventData ptrEvent)
    {
        EventController.Event(clickSoundEvent);
    }

    #endregion

}
