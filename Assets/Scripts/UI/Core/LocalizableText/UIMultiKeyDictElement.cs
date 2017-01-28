/*
 * Author(s): Isaiah Mann
 * Description: Used to join multiple dict terms together
 * Usage: [no notes]
 */

using UnityEngine;

public class UIMultiKeyDictElement : UIDictElement 
{	
	[Tooltip("This string is inserted between each term")]
	[SerializeField]
	string joinString;

	[SerializeField]
	protected string[] termKey;

	#region UIDictElement Overrides 

	protected override void setTextFromDict()
	{
		if(termKey.Length > 0)
		{
			bool hasJoinString = !string.IsNullOrEmpty(joinString);
			string term = string.Empty;
			for(int i = 0; i < termKey.Length; i++)
			{
				if(!string.IsNullOrEmpty(termKey[i]))
				{
					term += termKey[i];
					if(hasJoinString && i < termKey.Length - 1)
					{
						term += joinString;
					}
				}
			}
			SetText(term);
		}
	}

	#endregion

}
