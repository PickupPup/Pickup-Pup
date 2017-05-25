/*
 * Author(s): Isaiah Mann
 * Description: Controls an in game button
 * Usage: Should be attached to a GameObject with a button on it or in its children
 */

using UnityEngine;
using UnityEngine.UI;

using k = PPGlobal;

public class PPUIButton : PPUIElement 
{
	protected Button button;

    [SerializeField]
    bool createsPopup;

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

    public void SetImageToChild()
    {
        Image tempValue = this.image;
        try
        {
            this.image = GetComponentsInChildren<Image>()[1];
        }
        finally
        {
            if(!this.image)
            {
                this.image = tempValue;
            }
        }
    }

    public void ToggleEnabled(bool isEnabled)
    {
		checkReferences();
        this.button.interactable = isEnabled;
    }

	protected virtual void onClick()
	{
		callOnClickAction();
	}

    protected virtual bool shouldPlayClickSFX()
    {
        return !createsPopup;
    }

	void callOnClickAction()
	{
		if(onClickAction != null)
		{
            if(shouldPlayClickSFX())
            {
                EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
            }
			onClickAction();
		}
	}

}
