/*
 * Author(s): Isaiah Mann
 * Description: Describes a simple button class
 * Usage: Attach to a GameObject with Unity Canvas button on it
 */

using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIElement
{
	protected bool hasButtonGraphic
	{
		get
		{
			return buttonGraphic != null;
		}
	}

	protected Button button;
	protected Image buttonGraphic;
	protected MonoAction clickAction;
	protected Color selectedColor = Color.gray;
	protected Color deselectedColor = Color.white;

	public void SubscribeToClick(MonoAction action)
	{
		this.clickAction += action;
	}

	public void UnsubscribeFromClick(MonoAction action)
	{
		this.clickAction -= action;
	}

	public void UnsubscribeAllClickActions()
	{
		this.clickAction = null;
	}

	public void Select()
	{
		if(buttonGraphic)
		{
			buttonGraphic.color = selectedColor;
		}
	}

	public void Deselect()
	{
		if(buttonGraphic) 
		{
			buttonGraphic.color = deselectedColor;
		}
	}

	public void ToggleInteractable(bool isInteractable)
	{
		button.interactable = isInteractable;
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button = ensureReference<Button>();
		if(!button.targetGraphic)
		{
			button.targetGraphic = getTopImage();
		}
		buttonGraphic = button.image;
		button.onClick.AddListener(executeClick);
		if(hasButtonGraphic)
		{
			setButtonColors();
		}
	}

	#endregion

	void setButtonColors()
	{
		selectedColor = Color.Lerp(buttonGraphic.color, Color.gray, 0.5f);
		deselectedColor = buttonGraphic.color;
	}

	protected virtual void executeClick() 
	{
		if(clickAction != null) 
		{
			clickAction();
		}
	}

}
