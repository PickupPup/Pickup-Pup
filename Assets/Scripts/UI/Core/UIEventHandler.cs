/*
 * Author: Isaiah Mann
 * Description: Handles and triggers UI events
 */

using UnityEngine;

[System.Serializable]
public class UIEventHandler 
{
	[SerializeField]
	UIAction action;

	[SerializeField]
	string[] triggers;

	[SerializeField]
	bool triggerOnStart;

	public bool RespondsToTrigger(string trigger) 
	{
		return StringUtil.OrEquals(trigger, this.triggers);
	}

	public bool RunsOnStart()
	{
		return triggerOnStart;
	}

	public void Execute(UIElement element) 
	{
		action.Execute(element);
	}

}
