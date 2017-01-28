/*
 * Author: Isaiah Mann
 * Description: Hides element on event
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Hide", menuName = "UIEvent/Hide", order = 1)]
public class UIHideAction : UIAction 
{
	#region UIAction Overrides

	public override void Execute(UIElement target)
	{
		target.Hide();	
	}

	#endregion

}
