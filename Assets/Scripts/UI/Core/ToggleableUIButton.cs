/*
 * Author(s): Isaiah Mann
 * Description: Describes a button behaviour which can be toggled and off
 */

using UnityEngine;
using UnityEngine.UI;

public class ToggleableUIButton : UILabledButton 
{
	#region Instance Acccessors

	public bool IsToggled 
	{
		get 
		{
			return toggled;
		}
	}

	#endregion

	Color buttonUnselectedColor;
	Color buttonSelectedColor;

	[SerializeField]
	bool shouldToggleImage;
	[SerializeField]
	Image toggleImage;
	[SerializeField]
	Sprite imageOn;
	[SerializeField]
	Sprite imageOff;

	[SerializeField]
	bool shouldShowToggle;
	bool toggled = false;
	MonoAction toggleOffAction;
	Image buttonImage;

	public void SubscribeToggleOffAction(MonoAction toggleAction) 
	{
		toggleOffAction += toggleAction;
	}

	public void UnsubscribeToggleOffAction(MonoAction toggleAction) 
	{
		toggleOffAction -= toggleAction;
	}
		
	public virtual void Toggle() 
	{
		toggled = !toggled;
		if(shouldShowToggle) 
		{
			showToggle(toggled);
		}
	}

	protected virtual void showToggle(bool isToggled)
	{
		if(isToggled) 
		{
			if(shouldToggleImage) 
			{
				toggleImage.sprite = imageOn;
			}
			buttonImage.color = buttonSelectedColor;
		}
		else 
		{
			if(shouldToggleImage) 
			{
				toggleImage.sprite = imageOff;
			}
			buttonImage.color = buttonUnselectedColor;
		}
	}

	protected virtual void setToggle(bool isToggled)
	{
		toggled = isToggled;
	}

	protected override void executeClick() 
	{
		Toggle();
		if(toggled)
		{
			base.executeClick();
		} 
		else 
		{
			executeToggleOff();
		}
	}

	protected virtual void executeToggleOff()
	{
		if(toggleOffAction != null) 
		{
			toggleOffAction();
		}
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		buttonImage = GetComponent<Image>();
		buttonUnselectedColor = buttonImage.color;
		buttonSelectedColor = Color.Lerp(buttonImage.color, Color.black, 0.5f);
	}

	#endregion

}
