/*
 * Author: Isaiah Mann
 * Description: Used to receive events
 */

using UnityEngine;
using System.Collections;

public class UIEventListener : MonoBehaviourExtended 
{
	[SerializeField]
	UIEventHandler[] handlers;
	UIElement element;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		element = GetComponent<UIElement>();
		if(!element) 
		{
			element = gameObject.AddComponent<UIElement>();
		}
	}

	protected override void handleNamedEvent(string eventName) 
	{
		base.handleNamedEvent(eventName);
		foreach(UIEventHandler handler in handlers) 
		{
			if(handler.RespondsToTrigger(eventName)) 
			{
				handler.Execute(element);
			}
		}
	}

	#endregion

}
