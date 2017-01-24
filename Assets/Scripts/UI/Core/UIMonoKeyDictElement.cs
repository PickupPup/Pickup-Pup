/*
 * Author(s): Isaiah Mann
 * Description: Used to set text from a single dict term
 * Usage: [no notes]
 */

using UnityEngine;

public class UIMonoKeyDictElement : UIDictElement 
{	
	[SerializeField]
	protected string termKey;

	#region UIDictElement Overrides 

	protected override void setTextFromDict()
	{
		if(!string.IsNullOrEmpty(termKey))
		{
			setTextFromKey(termKey);
		}
	}

	#endregion
		
}
