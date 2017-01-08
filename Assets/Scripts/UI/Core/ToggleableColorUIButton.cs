/*
 * Author(s): Isaiah Mann
 * Description: Describes a button behaviour which can be toggled and off and its color changes upon toggle
 */

using UnityEngine;

public class ToggleableColorUIButton : ToggleableUIButton 
{
	[SerializeField]
	Color onColor;
	[SerializeField]
	Color offColor;

	protected override void showToggle(bool isToggled)
	{
		base.showToggle(isToggled);
		button.image.color = isToggled ? onColor : offColor;
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		button.image.color = offColor;
	}

	#endregion

}
