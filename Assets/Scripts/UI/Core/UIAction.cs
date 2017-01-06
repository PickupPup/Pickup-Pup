/*
 * Author: Isaiah Mann 
 * Description: A serializable UI action
 */

using UnityEngine;

public class UIAction : ScriptableObject 
{
	public virtual void Execute(UIElement target)
	{
		// Should be overriden in any subclass
		Debug.LogFormat("Executing UI action on {0}", target);
	}

}
