/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles the buttons for the DogBrowser
 * Usage: Should be a one to one relationship between each DogBrowser and its respective ButtonController
 */

using UnityEngine;
using k = PPGlobal;

public class DogBrowserButtonController : PPUIButtonController 
{	
	#region Instance Accessors

	public bool IsInitialized
	{
		get; 
		private set;
	}

	public bool HasSelectedPage
	{
		get
		{
			return selectedPageButton != null;
		}
	}

	#endregion

	int currentPageIndex
	{
		get
		{
			return parentWindow.CurrentPageIndex;
		}
	}

	int numPages
	{
		get
		{
			return parentWindow.GetNumPages();
		}
	}

	[SerializeField]
	GameObject pageButtonRef;
	[SerializeField]
	Transform pageButtonParent;

	[SerializeField]
	UIButton pageBackwardButton;
	[SerializeField]
	UIButton pageForwardButton;
	[SerializeField]
	UIButton rehomeButton;

	PPData.DogAction onDogClick;

	DogBrowser parentWindow;
	ToggleableColorUIButton[] pageButtons;
	ToggleableColorUIButton selectedPageButton;

    bool shouldRefresh;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		// GetComponentInParent also checks the current GameObject (as per the current prefab)
		parentWindow = GetComponentInParent<DogBrowser>();
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		if(!IsInitialized)
		{
			setupButtons();
		}
	}

    protected override void OnEnable()
    {
        base.OnEnable();
        updatePageButtons();
    }

	#endregion
        
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
        checkReferences();
        updatePageButtons();
		if(HasSelectedPage)
		{
			// Turn off the last page button 
			selectedPageButton.Toggle();
		}
		selectedPageButton = pageButtons[pageIndex];
		if(!onClickPageButton)
		{
			selectedPageButton.Toggle();
		}
	}

	public void Init(DogBrowser browser)
	{
		this.parentWindow = browser;
		setupButtons();
		IsInitialized = true;
	}

    public void RefreshPages()
    {
        this.shouldRefresh = true;
    }

	void setupButtons()
	{
	    rehomeButton.SubscribeToClick(parentWindow.OpenRehomeScreen);
        refreshPageButtonReferences();
        setupPageNavigation();
		IsInitialized = true;
	}

    void setupPageNavigation()
    {
        maintainCorrectPageButtonCount(onInit:true);
        parentWindow.SwitchToDefaultPage(onClickPageButton:false);
        for(int i = 0; i < pageButtons.Length; i++)
        {
            setupPageButton(pageButtons[i], i);
        }
        if(numPages > SINGLE_VALUE)
        {
            refreshNavigationButtons();
        }
        else
        {
            pageBackwardButton.Hide();
            pageForwardButton.Hide();
            pageButtonParent.gameObject.SetActive(false);
        }
    }

    void updatePageButtons()
    {
        if(shouldRefresh && referencesFetched)
        {
            maintainCorrectPageButtonCount(onInit:false);
            shouldRefresh = false;
        }
    }

    void refreshNavigationButtons()
    {
        // Need to clear them to fresh
        pageBackwardButton.UnsubscribeAllClickActions();
        pageForwardButton.UnsubscribeAllClickActions();
        pageBackwardButton.SubscribeToClick(parentWindow.PageBackward);
        pageForwardButton.SubscribeToClick(parentWindow.PageForward);
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
		int pages = numPages;
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
            pageButtons[i].gameObject.SetActive(false);
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
				parentWindow.SwitchToPage(pageIndex, onClickPageButton:true);
			});
		pageButton.SetToggleOnClickEnabled(isEnabled:false);
	}

	void refreshPageButtonReferences()
	{
        pageButtons = GetComponentsInChildren<ToggleableColorUIButton>(includeInactive:true);
		parentWindow.RefreshPageInitChecks(pageButtons.Length);
	}
 
	public void HandleDogSlotClick(Dog dog)
	{
		if(onDogClick != null)
		{
			onDogClick(dog);
		}
	}

}
