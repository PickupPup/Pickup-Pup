/*
 * Author(s): Isaiah Mann
 * Description: Describes a button with a text label
 */

using UnityEngine.UI;

public class UILabeledButton : UIButton 
{
	public void Set(string text, MonoAction clickAction, bool clearPreviousClickActions = true) 
	{
		if(clearPreviousClickActions) 
		{
			UnsubscribeAllClickActions();
		}
		SetText(text);
		SubscribeToClick(clickAction);
	}

}
