﻿/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

using UnityEngine;
using k = PPGlobal;

public class DogBrowser : PPUIElement 
{	
	const int MIN_PAGES = SINGLE_VALUE;

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
			return buttonController.HasSelectedPage;
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
	[SerializeField]
	DogBrowserType browserMode = DogBrowserType.AdoptedDogs;

	DogBrowserButtonController buttonController;
	DogSlot[] dogSlots;
	int currentlySelectedPageIndex = INVALID_VALUE;
	DogDatabase database;
	Dog[] dogCollection;
	bool[] pagesInitializedCheck;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		// Also checks the current GameObject (as per the current prefab setup)
		buttonController = GetComponentInChildren<DogBrowserButtonController>();
		dogSlots = GetComponentsInChildren<DogSlot>();
		database = DogDatabase.GetInstance;
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		switch(browserMode)
		{
			case DogBrowserType.AdoptedDogs:
				DogDescriptor[] dogInfos = dataController.AdoptedDogs.ToArray();
				DogFactory dogFactory = new DogFactory(hideGameObjects:true);
				dogCollection = dogFactory.CreateGroup(dogInfos);
				break;
			case DogBrowserType.AllDogs:
				dogCollection = new Dog[database.Dogs.Length];
				break;
		}
		setupDogSlots();
	}

	#endregion

    void OnEnable()
    {
        EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
    }

	public void Open(int pageIndex = NONE_VALUE)
	{
		checkReferences();
		Show();
		SwitchToPage(pageIndex, onClickPageButton:false);
	}

	public void Close()
	{
        EventController.Event(k.GetPlayEvent(k.BACK));
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
                dogSlots[i].ClearSlot();
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
		switch(browserMode)
		{
			case DogBrowserType.AdoptedDogs:
				return Mathf.Clamp(dogCollection.Length / dogsPerPage, 1, dogCollection.Length);
			case DogBrowserType.AllDogs:
				return getNumPages(this.database);
			default:
				return NONE_VALUE;
		}
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
        EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
        SwitchToPage(getPageForwardIndex(currentlySelectedPageIndex), onClickPageButton:false);
	}

	int getPageForwardIndex(int currentPage)
	{
		return getPageIndexFromOffset(currentPage, 1);
	}

	public void PageBackward()
	{
        EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
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
		switch(browserMode)
		{
			case DogBrowserType.AdoptedDogs:
				return getAdoptedDogsForPage(pageIndex);
			case DogBrowserType.AllDogs:
				return getDogsForPageFromAllDogs(pageIndex);
			default:
				return new Dog[0];
		}
	}

	Dog[] getAdoptedDogsForPage(int pageIndex)
	{
		int startIndex = getStartIndex(pageIndex);
		if(IntUtil.InRange(startIndex, dogCollection.Length))
		{
			int endIndex = Mathf.Clamp(startIndex + dogsPerPage, 0, dogCollection.Length);
			return ArrayUtil.GetRange(dogCollection, startIndex, endIndex - startIndex);
		}
		else
		{
			return new Dog[0];
		}
	}

	Dog[] getDogsForPageFromAllDogs(int pageIndex)
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
			slot.SubscribeToClickWhenOccupied(buttonController.HandleDogSlotClick);
		}
	}

	bool onPage(int pageIndex)
	{
		return currentlySelectedPageIndex == pageIndex;
	}

}

public enum DogBrowserType
{
	AllDogs,
	AdoptedDogs
}
