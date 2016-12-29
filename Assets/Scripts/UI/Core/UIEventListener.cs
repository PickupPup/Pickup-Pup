/*
 * Author: Isaiah Mann
 * Description: Used to receive events
 */

using UnityEngine;
using System.Collections;

public class UIEventListener : MonoBehaviourExtended {
	[SerializeField]
	UIEventHandler[] handlers;
	UIElement element;

	protected override void SetReferences () {
		base.SetReferences ();
		element = GetComponent<UIElement>();
		if (!element) {
			element = gameObject.AddComponent<UIElement>();
		}
	}

	protected override void HandleNamedEvent (string eventName) {
		base.HandleNamedEvent(eventName);
		foreach (UIEventHandler handler in handlers) {
			if (handler.RespondsToTrigger(eventName)) {
				handler.Execute(element);
			}
		}
	}
}
