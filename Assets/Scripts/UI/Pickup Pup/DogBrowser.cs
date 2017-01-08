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
	[SerializeField]
	UIButton closeWindowHitArea;
	[SerializeField]
	UIButton rehomeButton;

	PPData.DogAction onDogClick;
	DogSlot[] dogSlots;
	ToggleableColorUIButton[] pageButtons;
	ToggleableColorUIButton selectedPageButton;
	int currentlySelectedPageIndex = INVALID_VALUE;

	#region MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		setupDogSlots();
		setupButtons();
	}

	#endregion

	public void Open(int pageIndex = NONE_VALUE)
	{
		Show();
		SwitchToPage(pageIndex, onClickPageButton:false);
	}

	public void Close()
	{
		Hide();
	}

	public void Set(Dog[] dogs) 
	{
		for(int i = 0; i < dogSlots.Length; i++)
		{
			dogSlots[i].Init(dogs[i]);
		}
	}
		
	public void SubscribeToDogClick (PPData.DogAction dogClickAction)
	{
		onDogClick += dogClickAction;
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

	// TODO:
	void openRehomeScreen()
	{
		Debug.Log("REHOME Screen has yet to be implemented");
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

	void setupDogSlots()
	{
		dogSlots = GetComponentsInChildren<DogSlot>();
		foreach(DogSlot slot in this.dogSlots)
		{
			slot.SubscribeToClick(handleDogSlotClick);
		}
	}

	void handleDogSlotClick(Dog dog)
	{
		if(onDogClick != null)
		{
			onDogClick(dog);
		}
	}

	void setupButtons()
	{
		closeWindowHitArea.SubscribeToClick(Close);
		rehomeButton.SubscribeToClick(openRehomeScreen);
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
