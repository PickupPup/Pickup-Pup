/*
 * Author: Isaiah Mann
 * Description: Stops UI element opacity from lerping
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "StopOpacityLerp", menuName = "UIEvent/StopOpacityLerp", order = 4)]
public class UIStopOpacityLerpAction : UIAction 
{
	#region UIAction Overrides

	public override void Execute(UIElement target) {
		target.StopOpacityLerp();
	}

	#endregion

}
