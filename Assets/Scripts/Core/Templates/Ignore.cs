/*
 * Author: Isaiah Mann
 * Description: Used to skip over operations in the MonoBehaviourExtended system
 */

public class Ignore : MonoBehaviourExtended 
{
	#region Instance Accessors

	public bool ColorChange 
	{
		get 
		{
			return colorChange;
		}
	}

	#endregion

	[UnityEngine.SerializeField]
	bool colorChange;

	protected override void cleanupReferences() 
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

	protected override void setReferences()
	{
		// Nothing
	}

}
