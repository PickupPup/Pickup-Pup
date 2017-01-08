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

	int dogsPerPage
	{
		get
		{
			return dogSlots.Length;
		}
	}

	[SerializeField]
	GameObject pageButtonRef;
	[SerializeField]
	Transform pageButtonParent;
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
	DogDatabase database;
	Dog[] dogCollection;
	bool[] pagesInitializedCheck;

	#region MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		dogSlots = GetComponentsInChildren<DogSlot>();
		database = DogDatabase.GetInstance;
		dogCollection = new Dog[database.Dogs.Length];
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
			if(ArrayUtil.InRange(dogs, i) && dogs[i] != null)
			{
				dogSlots[i].Show();
				dogSlots[i].Init(dogs[i]);
			}
			else
			{
				dogSlots[i].Hide();
			}
		}
	}
		
	public void SubscribeToDogClick(PPData.DogAction dogClickAction)
	{
		onDogClick += dogClickAction;
	}

	public void UnsubscribeFromDogClick(PPData.DogAction dogClickAction)
	{
		onDogClick -= dogClickAction;
	}

	public void SwitchToPage(int pageIndex, bool onClickPageButton)
	{
		this.currentlySelectedPageIndex = pageIndex;
		if (hasSelectedPage)
		{
			// Turn off the last page button 
			selectedPageButton.Toggle();
		}
		selectedPageButton = pageButtons[pageIndex];
		if (!onClickPageButton)
		{
			selectedPageButton.Toggle();
		}
		Set(getDogsForPage(pageIndex));
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

	int getNumPages(DogDatabase database)
	{
		return Mathf.CeilToInt((float) database.Dogs.Length / (float) dogsPerPage);
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
		if(ArrayUtil.InRange(pagesInitializedCheck, pageIndex))
		{
			if(pagesInitializedCheck[pageIndex])
			{
				int startIndex = getStartIndex(pageIndex);
				return getRangeFromDogCollection(startIndex, getDogsOnPage(pageIndex));
			}
			else
			{
				return loadDogsForPage(pageIndex);
			}
		}
		else
		{
			return new Dog[0];
		}
	}
		
	Dog[] loadDogsForPage(int pageIndex)
	{
		if(ArrayUtil.InRange(pagesInitializedCheck, pageIndex))
		{
			int dogsOnPage = getDogsOnPage(pageIndex);
			int startIndex = getStartIndex(pageIndex);
			DogDescriptor[] dogInfos = database.GetDogRange(startIndex, dogsOnPage);
			Dog[] matchingDogs = new DogFactory(hideGameObjects:true).CreateGroup(dogInfos);
			ArrayUtil.CopyRange(matchingDogs, dogCollection, 0, startIndex, dogsOnPage);
			pagesInitializedCheck[pageIndex] = true;
			return matchingDogs;
		}
		else
		{
			return new Dog[0];
		}
	}

	int getDogsOnPage(int pageIndex)
	{
		int startIndex = getStartIndex(pageIndex);
		if(startIndex + dogsPerPage < database.Dogs.Length)
		{
			return dogsPerPage;
		}
		else 
		{
			return database.Dogs.Length - startIndex;
		}
	}

	Dog[] getRangeFromDogCollection(int start, int length)
	{
		return ArrayUtil.GetRange(this.dogCollection, start, length);
	}

	int getStartIndex(int pageIndex)
	{
		return pageIndex * dogsPerPage;
	}

	void setupDogSlots()
	{
		foreach(DogSlot slot in this.dogSlots)
		{
			// Links together DogSlot and UIButton scripts
			slot.GetComponent<UIButton>().SubscribeToClick(slot.ExecuteClick);
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

	bool onPage(int pageIndex)
	{
		return currentlySelectedPageIndex == pageIndex;
	}

	void setupButtons()
	{
		closeWindowHitArea.SubscribeToClick(Close);
		rehomeButton.SubscribeToClick(openRehomeScreen);
		refreshPageButtonReferences();
		maintainCorrectPageButtonCount(onInit:true);
		SwitchToPage(defaultStartPage, onClickPageButton:false);
		for(int i = 0; i < pageButtons.Length; i++)
		{
			setupPageButton(pageButtons[i], i);
		}
		pageBackwardButton.SubscribeToClick(PageBackward);
		pageForwardButton.SubscribeToClick(PageForward);
	}

	// There are extra steps that do not need to be performed on init
	ToggleableColorUIButton addPageButton(int pageIndex, bool addingOnInit)
	{
		GameObject buttonobject = Instantiate(pageButtonRef, pageButtonParent);
		ToggleableColorUIButton pageButton = buttonobject.GetComponent<ToggleableColorUIButton>();
		if(!addingOnInit)
		{
			setupPageButton(pageButton, pageIndex);
			refreshPageButtonReferences();
		}
		return pageButton;
	}

	void maintainCorrectPageButtonCount(bool onInit)
	{
		int pages = getNumPages(this.database);
		if(pages > pageButtons.Length)
		{
			padPageButtons(pages, onInit);
		}
		else if(pages < pageButtons.Length)
		{
			trimPageButtons(pages);
		}
		refreshPageButtonReferences();
	}

	void trimPageButtons(int desiredLength)
	{
		for(int i = desiredLength; i < pageButtons.Length; i++)
		{
			Destroy(pageButtons[i].gameObject);
		}
	}

	void padPageButtons(int desiredLength, bool onInit)
	{
		int currentButtonCount = pageButtons.Length;
		for(int i = currentButtonCount; i < desiredLength; i++)
		{
			addPageButton(i, onInit);
		}
	}

	void setupPageButton(ToggleableColorUIButton pageButton, int pageIndex)
	{
		pageButton.SubscribeToClick(delegate() 
			{
				SwitchToPage(pageIndex, onClickPageButton:true);
			});
		pageButton.SetToggleOnClickEnabled(isEnabled:false);
	}

	void refreshPageButtonReferences()
	{
		pageButtons = GetComponentsInChildren<ToggleableColorUIButton>();
		bool needToCopyOldChecks = pagesInitializedCheck != null;
		bool[] oldChecks = pagesInitializedCheck;
		// Assume all bools in array default to false
		pagesInitializedCheck = new bool[pageButtons.Length];
		if(needToCopyOldChecks)
		{
			ArrayUtil.CopyRange(oldChecks, pagesInitializedCheck, 0, 0, oldChecks.Length);
		}
	}

}
