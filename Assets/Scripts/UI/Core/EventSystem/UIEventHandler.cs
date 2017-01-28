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

    [SerializeField]
    bool triggerOnSwipe;
    [SerializeField]
    Direction[] validSwipeDirections;

    [SerializeField]
    bool triggerOnClick;

	public bool RespondsToTrigger(string trigger) 
	{
		return StringUtil.OrEquals(trigger, this.triggers);
	}

	public bool RunsOnStart()
	{
		return triggerOnStart;
	}

    public bool RunsOnSwipe(Direction swipeDirection)
    {
        return triggerOnSwipe && ArrayUtil.Contains(validSwipeDirections, swipeDirection);
    }

    public bool RunsOnClick()
    {
        return triggerOnClick;
    }

	public void Execute(UIElement element) 
	{
		action.Execute(element);
	}

}
