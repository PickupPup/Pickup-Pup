using UnityEngine;

[CreateAssetMenuAttribute(fileName = "StopOpacityLerp", menuName = "UIEvent/StopOpacityLerp", order = 4)]
public class UIStopOpacityLerpAction : UIAction {
	public override void Execute (UIElement target) {
		target.StopOpacityLerp();
	}
}
