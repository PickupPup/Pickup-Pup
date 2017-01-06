/*
 * Author: Isaiah Mann
 * Description: Lerps an assets opacity in and out
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "OpacityLerp", menuName = "UIEvent/OpacityLerp", order = 3)]
public class UIOpacityLerpAction : UIAction 
{
	[SerializeField]
	float startOpacity;
	[SerializeField]
	float endOpacity;
	[SerializeField]
	float time;
	[SerializeField]
	bool loop;

	#region UIAction Overrides

	public override void Execute(UIElement target) 
	{
		target.StartOpacityLerp(startOpacity, endOpacity, time, loop);
	}

	#endregion

}
