/*
 * Author(s): Isaiah Mann
 * Description: Controls an in game button
 * Usage: Should be attached to a GameObject with a button on it or in its children
 */

using UnityEngine.UI;
using k = PPGlobal;

public class PPUIButton : PPUIElement 
{
	protected Button button;

	MonoAction onClickAction;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button = GetComponentInChildren<Button>();
		button.onClick.AddListener(onClick);
	}

	#endregion

	public void SubscribeToClick(MonoAction clickAction)
	{
		this.onClickAction += clickAction;
	}

	public void UnsubscribeFromClick(MonoAction clickAction)
	{
		this.onClickAction -= clickAction;
	}

	protected virtual void onClick()
	{
		callOnClickAction();
	}

	void callOnClickAction()
	{
		if(onClickAction != null)
		{
            EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
			onClickAction();
		}
	}

}
