/*
 * Author(s): Isaiah Mann
 * Description: Meta class that all controllers inherit from (all controllers are singletons)
 */

using UnityEngine;

public abstract class Controller : MonoBehaviourExtended 
{
	protected override void setReferences() 
	{
		// Nothing
	}

	protected override void fetchReferences() 
	{
		// Nothing
	}

	protected override void handleNamedEvent(string eventName) 
	{
		// Nothing
	}

	protected override void cleanupReferences() 
	{
		// Nothing
	}

}
