/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

using UnityEngine;

public class DogBrowser : PPUIElement 
{	
	[SerializeField]
	int defaultStartPage;
	DogSlot[] elements;
	ToggleableColorUIButton[] pageButtons;

	#region MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		elements = GetComponentsInChildren<DogSlot>();
		pageButtons = GetComponentsInChildren<ToggleableColorUIButton>();
		pageButtons[defaultStartPage].Toggle();
	}

	#endregion

	public void Set(Dog[] dogs) 
	{
		for(int i = 0; i < elements.Length; i++)
		{
			elements[i].Init(dogs[i]);
		}
	}

	public void SwitchToPage(int pageIndex)
	{
		throw new System.NotImplementedException();
	}

	protected Dog[] getDogsForPage(int pageIndex)
	{
		throw new System.NotImplementedException();
	}

}
