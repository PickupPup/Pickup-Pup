/*
 * Author: Isaiah Mann
 * Description: Shows a hidden UI element
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Show", menuName = "UIEvent/Show", order = 0)]
public class UIShowAction : UIAction 
{
	#region UIAction Overrides

	public override void Execute(UIElement target)
	{
		target.Show();
	}

	#endregion

}
