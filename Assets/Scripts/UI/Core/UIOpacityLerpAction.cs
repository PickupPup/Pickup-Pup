/*
 * Author: Isaiah Mann
 * Description: Lerps an assets opacity in and out
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "OpacityLerp", menuName = "UIEvent/OpacityLerp", order = 3)]
public class UIOpacityLerpAction : UIAction 
{
	
	public float StartOpacity;
	public float EndOpacity;
	public float Time;
	public bool Loop;

	#region UIAction Overrides

	public override void Execute(UIElement target) 
	{
		target.StartOpacityLerp(StartOpacity, EndOpacity, Time, Loop);
	}

	#endregion

}
