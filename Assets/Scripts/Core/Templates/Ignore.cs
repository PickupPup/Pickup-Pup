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

	protected override void CleanupReferences() 
	{
		// Nothing
	}

	protected override void FetchReferences() 
	{
		// Nothing
	}

	protected override void HandleNamedEvent(string eventName) 
	{
		// Nothing
	}

	protected override void SetReferences()
	{
		// Nothing
	}

}
