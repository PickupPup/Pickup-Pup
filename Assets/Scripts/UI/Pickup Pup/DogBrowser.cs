/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

using UnityEngine;

public class DogBrowser : PPUIElement 
{	
	bool hasSelectedPage
	{
		get
		{
			return selectedPageButton != null;
		}
	}

	[SerializeField]
	int defaultStartPage;

	DogSlot[] elements;
	ToggleableColorUIButton[] pageButtons;
	ToggleableColorUIButton selectedPageButton;

	#region MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		elements = GetComponentsInChildren<DogSlot>();
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		setupPageButtons();
	}

	#endregion

	public void Set(Dog[] dogs) 
	{
		for(int i = 0; i < elements.Length; i++)
		{
			elements[i].Init(dogs[i]);
		}
	}

	public void SwitchToPage(int pageIndex, bool onClick)
	{
		if (hasSelectedPage)
		{
			// Turn off the last page bittpm 
			selectedPageButton.Toggle();
		}
		selectedPageButton = pageButtons[pageIndex];
		if (!onClick)
		{
			selectedPageButton.Toggle();
		}
	}

	protected Dog[] getDogsForPage(int pageIndex)
	{
		throw new System.NotImplementedException();
	}

	protected void setupPageButtons()
	{
		pageButtons = GetComponentsInChildren<ToggleableColorUIButton>();
		SwitchToPage(defaultStartPage, onClick:false);
		for(int i = 0; i < pageButtons.Length; i++)
		{
			int pageIndex = i;
			pageButtons[i].SubscribeToClick(delegate() 
				{
					SwitchToPage(pageIndex, onClick:true);
				});
		}
	}

}
