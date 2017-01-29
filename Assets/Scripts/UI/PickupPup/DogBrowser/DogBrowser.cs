/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

using UnityEngine;
using System.Collections.Generic;
using k = PPGlobal;
using System.Linq;

public class DogBrowser : PPUIElement, IPageable
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
	
    public bool IsSinglePageBrowser
    {
        get
        {
            return GetNumPages() == SINGLE_VALUE;
        }
    }

    #region IPageable Interface

    bool IPageable.PageWrapAllowed
    {
        get
        {
            return !IsSinglePageBrowser;
        }
    }

    bool IPageable.CanPageForward
    {
        get
        {
            return !IsSinglePageBrowser;
        }
    }

    bool IPageable.CanPageBackward
    {
        get
        {
            return !IsSinglePageBrowser;
        }
    }

    #endregion

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
	List<Dog> dogCollection;
	bool[] pagesInitializedCheck;
	bool inScoutingSelectMode = false;

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
        updateDogCollection();
		setupDogSlots();
	}

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        EventController.Subscribe(handleAdoptEvent);
    }

    #endregion

    void OnEnable()
    {
        EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
    }
		
	public void Open(bool inScoutingSelectMode, int pageIndex = NONE_VALUE)
	{
		this.inScoutingSelectMode = inScoutingSelectMode;
		checkReferences();
		Show();
		SwitchToPage(pageIndex, onClickPageButton:false);
	}

    #region UIElement Overrides

    public override void Hide()
	{
        EventController.Event(k.GetPlayEvent(k.BACK));
		base.Hide();
	}

    #endregion

    public void Set(Dog[] dogs) 
	{
        int currentDogIndex = 0;
		for(int i = 0; i < dogSlots.Length; i++)
		{
            // Filter out scoting dogs:
            if(inScoutingSelectMode)
            {
                while(currentDogIndex < dogs.Length && dogs[currentDogIndex].IsScouting)
                {
                    currentDogIndex++;
                }
            }
            if(ArrayUtil.InRange(dogs, currentDogIndex) && dogs[currentDogIndex] != null)
			{
				dogSlots[i].Show();
                dogSlots[i].Init(dogs[currentDogIndex++], inScoutingSelectMode);
			}
			else
			{
                dogSlots[i].ClearSlot();
			}
		}
	}

    public void Set(List<Dog> dogs)
    {
        Set(dogs.ToArray());
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
                return getNumPages(this.dogCollection);
			case DogBrowserType.AllDogs:
                return getNumPages(this.database.Dogs);
			default:
				return NONE_VALUE;
		}
	}

	int getNumPages<T>(T[] dogs)
	{
        return Mathf.Clamp(Mathf.CeilToInt((float) dogs.Length / (float) dogsPerPage), 1, dogs.Length);
	}

    int getNumPages<T>(List<T> dogs)
    {
        return Mathf.Clamp(Mathf.CeilToInt((float) dogs.Count / (float) dogsPerPage), 1, dogs.Count);
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
        return mod(rawIndex, GetNumPages());
	}

    void handleAdoptEvent(string eventName, Dog dog)
    {
        if (eventName == k.ADOPT && !dogCollection.Contains(dog))
        {
            updateDogCollection();
        }
    }

    List<Dog> getDogsForPage(int pageIndex)
	{
		switch(browserMode)
		{
			case DogBrowserType.AdoptedDogs:
				return getAdoptedDogsForPage(pageIndex);
			case DogBrowserType.AllDogs:
				return getDogsForPageFromAllDogs(pageIndex);
			default:
				return new List<Dog>();
		}
	}

	List<Dog> getAdoptedDogsForPage(int pageIndex)
	{
		int startIndex = getStartIndex(pageIndex);
		if(IntUtil.InRange(startIndex, dogCollection.Count))
		{
			int endIndex = Mathf.Clamp(startIndex + dogsPerPage, 0, dogCollection.Count);
			return dogCollection.GetRange(startIndex, endIndex - startIndex);
		}
		else
		{
			return new List<Dog>();
		}
	}

	List<Dog> getDogsForPageFromAllDogs(int pageIndex)
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
			return new List<Dog>();
		}
	}

	List<Dog> loadDogsForPage(int pageIndex)
	{
		if(ArrayUtil.InRange(pagesInitializedCheck, pageIndex))
		{
			int dogsOnPage = getDogsOnPage(pageIndex);
			int startIndex = getStartIndex(pageIndex);
			List<DogDescriptor> dogInfos = database.GetDogRangeList(startIndex, dogsOnPage);
			List<Dog> matchingDogs = new DogFactory(hideGameObjects:true).CreateGroupList(dogInfos);
			ListUtil.CopyRange(dogCollection, matchingDogs, 0, startIndex, dogsOnPage);
			pagesInitializedCheck[pageIndex] = true;
			return matchingDogs;
		}
		else
		{
			return new List<Dog>();
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

	List<Dog> getRangeFromDogCollection(int start, int length)
	{
		return dogCollection.GetRange(start, length);
	}

    void updateDogCollection()
    {
        dogCollection = new List<Dog>();
        switch (browserMode)
        {
            case DogBrowserType.AdoptedDogs:
                List<DogDescriptor> dogInfos = dataController.AdoptedDogs;
                DogFactory dogFactory = new DogFactory(hideGameObjects: true);
                dogCollection = dogFactory.CreateGroupList(dogInfos);
                break;
            case DogBrowserType.AllDogs:
                // Nothing
                break;
        }
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
