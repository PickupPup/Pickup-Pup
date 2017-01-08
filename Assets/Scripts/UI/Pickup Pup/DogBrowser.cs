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
	[SerializeField]
	UIButton pageBackwardButton;
	[SerializeField]
	UIButton pageForwardButton;

	DogSlot[] elements;
	ToggleableColorUIButton[] pageButtons;
	ToggleableColorUIButton selectedPageButton;
	int currentlySelectedPageIndex = INVALID_VALUE;

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
		
	public void SwitchToPage(int pageIndex, bool onClickPageButton)
	{
		this.currentlySelectedPageIndex = pageIndex;
		if (hasSelectedPage)
		{
			// Turn off the last page bittpm 
			selectedPageButton.Toggle();
		}
		selectedPageButton = pageButtons[pageIndex];
		if (!onClickPageButton)
		{
			selectedPageButton.Toggle();
		}
		checkDirectionalButtons();
	}

	public void PageForward()
	{
		if(canPageForward())	
		{
			SwitchToPage(currentlySelectedPageIndex + 1, onClickPageButton:false);
		}
	}

	public void PageBackward()
	{
		if(canPageBackward())
		{
			SwitchToPage(currentlySelectedPageIndex - 1, onClickPageButton:false);
		}
	}
		
	bool canPageForward()
	{
		return this.currentlySelectedPageIndex < this.pageButtons.Length - 1;
	}

	bool canPageBackward()
	{
		return this.currentlySelectedPageIndex > 0;
	}

	void checkDirectionalButtons()
	{
		pageForwardButton.ToggleInteractable(canPageForward());
		pageBackwardButton.ToggleInteractable(canPageBackward());
	}

	Dog[] getDogsForPage(int pageIndex)
	{
		throw new System.NotImplementedException();
	}

	void setupPageButtons()
	{
		pageButtons = GetComponentsInChildren<ToggleableColorUIButton>();
		SwitchToPage(defaultStartPage, onClickPageButton:false);
		for(int i = 0; i < pageButtons.Length; i++)
		{
			int pageIndex = i;
			pageButtons[i].SubscribeToClick(delegate() 
				{
					SwitchToPage(pageIndex, onClickPageButton:true);
				});
		}
		pageBackwardButton.SubscribeToClick(PageBackward);
		pageForwardButton.SubscribeToClick(PageForward);
	}

}
