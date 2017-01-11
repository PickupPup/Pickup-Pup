/*
 * Author(s): Isaiah Mann
 * Description: Describes a button behaviour which can be toggled and off
 */

using UnityEngine;
using UnityEngine.UI;

public class ToggleableUIButton : UILabeledButton 
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
	bool toggleOffOnClickEnabled = true;
	MonoAction toggleOffAction;
	Image buttonImage;

	public void SetToggleOnClickEnabled(bool isEnabled)
	{
		this.toggleOffOnClickEnabled = isEnabled;
	}

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
		if(toggled && toggleOffOnClickEnabled)
		{
			executeToggleOff();
		} 
		else if(!toggled)
		{
			base.executeClick();
		}
		else 
		{
			// End control early so button doesn't toggle off
			return;
		}
		Toggle();
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
