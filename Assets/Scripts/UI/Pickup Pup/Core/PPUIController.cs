/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

public class PPUIController : MonoBehaviourExtended 
{
	protected PPSceneController sceneController;
    protected PPGameController gameController;

	#region MonoBehaviourExtended Overrides

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
	}

}
