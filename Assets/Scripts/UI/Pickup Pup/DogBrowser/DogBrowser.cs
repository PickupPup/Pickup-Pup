/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

 using UnityEngine;

public class DogBrowser : PPUIElement 
{	
	#region Instance Accessors

	public int CurrentPageIndex
	{
		get
		{
			return this.currentlySelectedPageIndex;
		}
	}
		
	#endregion

	bool hasSelectedPage
	{
		get
		{
			return buttonController.hasSelectedPage;
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
	int defaultStartPage;

	DogBrowserButtonController buttonController;
	DogSlot[] dogSlots;
	int currentlySelectedPageIndex = INVALID_VALUE;
	DogDatabase database;
	Dog[] dogCollection;
	bool[] pagesInitializedCheck;

	#region MonoBehaviourExtended 

	protected override void setReferences()
	{
		base.setReferences();
		// Also checks the current GameObject (as per the current prefab setup)
		buttonController = GetComponentInChildren<DogBrowserButtonController>();
		dogSlots = GetComponentsInChildren<DogSlot>();
		database = DogDatabase.GetInstance;
		dogCollection = new Dog[database.Dogs.Length];
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		setupDogSlots();
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
		buttonController.SubscribeToDogClick(dogClickAction);
	}

	public void UnsubscribeFromDogClick(PPData.DogAction dogClickAction)
	{
		buttonController.UnsubscribeFromDogClick(dogClickAction);
	}

	public void SwitchToDefaultPage(bool onClickPageButton)
	{
		this.SwitchToPage(defaultStartPage, onClickPageButton);
	}

	public void SwitchToPage(int pageIndex, bool onClickPageButton)
	{
		this.currentlySelectedPageIndex = pageIndex;
		buttonController.SwitchToPage(pageIndex, onClickPageButton);
		Set(getDogsForPage(pageIndex));
	}

	public void RefreshPageInitChecks(int newPageButtonCount)
	{
		bool needToCopyOldChecks = pagesInitializedCheck != null;
		bool[] oldChecks = pagesInitializedCheck;
		// Assume all bools in array default to false
		pagesInitializedCheck = new bool[newPageButtonCount];
		if(needToCopyOldChecks)
		{
			ArrayUtil.CopyRange(oldChecks, pagesInitializedCheck, 0, 0, oldChecks.Length);
		}

	}

	public int GetNumPages()
	{
		return getNumPages(this.database);
	}

	int getNumPages(DogDatabase database)
	{
		return Mathf.CeilToInt((float) database.Dogs.Length / (float) dogsPerPage);
	}

	// TODO:
	public void OpenRehomeScreen()
	{
		Debug.Log("REHOME Screen has yet to be implemented");
	}
		
	public void PageForward()
	{
		SwitchToPage(getPageForwardIndex(currentlySelectedPageIndex), onClickPageButton:false);
	}

	int getPageForwardIndex(int currentPage)
	{
		return getPageIndexFromOffset(currentPage, 1);
	}

	public void PageBackward()
	{
		SwitchToPage(getPageBackwardIndex(currentlySelectedPageIndex), onClickPageButton:false);
	}

	int getPageBackwardIndex(int currentPage)
	{
		return getPageIndexFromOffset(currentPage, -1);
	}

	int getPageIndexFromOffset(int currentPage, int offset)
	{
		currentPage += offset;
		return getPageWrapIndex(currentPage);
	}

	int getPageWrapIndex(int rawIndex)
	{
		return mod(rawIndex, getNumPages(this.database));
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
			slot.SubscribeToClick(buttonController.HandleDogSlotClick);
		}
	}

	bool onPage(int pageIndex)
	{
		return currentlySelectedPageIndex == pageIndex;
	}

}
