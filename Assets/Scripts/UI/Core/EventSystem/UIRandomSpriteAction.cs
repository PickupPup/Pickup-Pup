/*
 * Author: Isaiah Mann
 * Description: Sets UI element's sprite to a new one from the sampling
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "RandomSprite", menuName = "UIEvent/RandomSprite", order = 2)]
public class UIRandomSpriteAction : UIAction 
{
	#region UIAction Overrides

	public override void Execute(UIElement target)
	{
		target.RandomSprite();
	}

	#endregion

}
