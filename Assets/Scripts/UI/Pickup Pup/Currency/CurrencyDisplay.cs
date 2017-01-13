/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls UI of Currency
 */

public class CurrencyDisplay : PPUIElement 
{
	CurrencyType type;
	PPDataController dataController;

    public void Init(PPDataController dataController, CurrencyType type)
	{
		unsubscribeEvents();
		this.dataController = dataController;
		this.type = type;
		subscribeEvents();
		switch(type)
		{
			case CurrencyType.Coins:
				updateAmount(dataController.Coins.Amount);
				break;
			case CurrencyType.DogFood:
				updateAmount(dataController.DogFood.Amount);
				break;
		}
	}


	#region MonoBehaviourExtended Overrides

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		if(dataController)
		{
			switch(type)
			{
				case CurrencyType.Coins:
					dataController.SubscribeToCoinsChange(updateAmount);
					break;
				case CurrencyType.DogFood:
					dataController.SubscribeToFoodChange(updateAmount);
					break;
			}
		}
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		if(dataController)
		{
			switch(type)
			{
				case CurrencyType.Coins:
					dataController.UnsubscribeFromCoinsChange(updateAmount);
					break;
				case CurrencyType.DogFood:
					dataController.UnsubscribeToFoodChange(updateAmount);
					break;
			}
		}
	}

	#endregion

	void updateAmount(int newAmount)
	{
		text.text = newAmount.ToString();
	}

}
