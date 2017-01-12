/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended 
{
	protected PPSceneController sceneController;
    protected PPGameController gameController;
    protected DogProfile dogProfile;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        dogProfile = GetComponentInChildren<DogProfile>();
        dogProfile.Hide();
    }

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		sceneController = PPSceneController.Instance;
		gameController = PPGameController.GetInstance;
    }

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		EventController.Subscribe(handlePPDogEvent);
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		EventController.Unsubscribe(handlePPDogEvent);
	}

	#endregion

    public void LoadStart() 
	{
		sceneController.LoadStart();
	}

	public void LoadHome()
	{
		sceneController.LoadHome();
	}

    public void LoadShelter()
    {
        sceneController.LoadShelter();
    }

    public void LoadShop()
    {
        sceneController.LoadShop();
    }

	void handlePPDogEvent(PPEvent gameEvent, Dog dog)
	{
		if(gameEvent == PPEvent.ClickDogSlot)
		{
			handleDogSlotClicked(dog);
		}
	}

	void handleDogSlotClicked(Dog dog)
	{
        // TODO: Insert universal dog slot handle code here
        showDogProfile(dog);
	}

    void showDogProfile(Dog dog)
    {
        dogProfile.SetProfile(dog);
        dogProfile.Show();
    }

}
