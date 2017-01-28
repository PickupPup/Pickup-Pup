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
	UIButton closeWindowHitArea;
	[SerializeField]
	UIButton rehomeButton;

	PPData.DogAction onDogClick;

	DogBrowser parentWindow;
	ToggleableColorUIButton[] pageButtons;
	ToggleableColorUIButton selectedPageButton;

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

	void setupButtons()
	{
		closeWindowHitArea.SubscribeToClick(parentWindow.Close);
		rehomeButton.SubscribeToClick(parentWindow.OpenRehomeScreen);
		refreshPageButtonReferences();
		maintainCorrectPageButtonCount(onInit:true);
		parentWindow.SwitchToDefaultPage(onClickPageButton:false);
		for(int i = 0; i < pageButtons.Length; i++)
		{
			setupPageButton(pageButtons[i], i);
		}
		if(numPages > SINGLE_VALUE)
		{
			pageBackwardButton.SubscribeToClick(parentWindow.PageBackward);
			pageForwardButton.SubscribeToClick(parentWindow.PageForward);
		}
		else
		{
            pageBackwardButton.Hide();
            pageForwardButton.Hide();
            pageButtonParent.gameObject.SetActive(false);
		}
		IsInitialized = true;
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
				parentWindow.SwitchToPage(pageIndex, onClickPageButton:true);
			});
		pageButton.SetToggleOnClickEnabled(isEnabled:false);
	}

	void refreshPageButtonReferences()
	{
		pageButtons = GetComponentsInChildren<ToggleableColorUIButton>();
		parentWindow.RefreshPageInitChecks(pageButtons.Length);
	}
 
	public void HandleDogSlotClick(Dog dog)
	{
		if(onDogClick != null)
		{
            EventController.Event(k.GetPlayEvent(k.MENU_CLICK));
			onDogClick(dog);
		}
	}

}
