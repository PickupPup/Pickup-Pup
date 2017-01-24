/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the display of multiple currencies on the Currency Panel
 */

using UnityEngine;

public class CurrencyPanel : SingletonController<CurrencyPanel>
{
    [SerializeField]
    CurrencyDisplay coinsDisplay;
    [SerializeField]
    CurrencyDisplay dogFoodDisplay;
	[SerializeField]
	UIElement giftTimerDisplay;

	PPTimer dailyGiftTimer;
    PPDataController dataController;

    #region MonoBehaviourExtended Overrides

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if (dataController)
        {
            dataController.SubscribeToCoinsChange(updateCoinsDisplay);
            dataController.SubscribeToFoodChange(updateDogFoodDisplay);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if (dataController)
        {
            dataController.UnsubscribeFromCoinsChange(updateCoinsDisplay);
            dataController.UnsubscribeFromFoodChange(updateDogFoodDisplay);
        }
    }

    #endregion

    public void Init(PPGameController gameController, PPDataController dataController)
    {
        unsubscribeEvents();
        this.dataController = dataController;
        subscribeEvents();

        // Display Updated Currency
        dogFoodDisplay.Init(dataController, dataController.DogFood);
        coinsDisplay.Init(dataController, dataController.Coins);
		initDailyGiftCountdown(gameController.Tuning, dataController);
    }

	void initDailyGiftCountdown(PPTuning tuning, PPDataController dataController)
	{
		float dailyGiftCountdown;
		if(dataController.DailyGiftCountdownRunning)
		{
			dailyGiftCountdown = dataController.DailyGiftCountdown;
		}
		else
		{
            // TODO: Fix
//			dailyGiftCountdown = tuning.dail;
		}
        // TODO: Fix
		// dailyGiftTimer = new PPTimer(dailyGiftCountdown, tuning.DefaultTimerTimeStepSec);
		dailyGiftTimer.SubscribeToTimeChange(handleDailyGiftCountDownChange);
		dataController.StartDailyGiftCountdown(dailyGiftTimer);
		dailyGiftTimer.Begin();
	}
		
	void handleDailyGiftCountDownChange(float timeRemaining)
	{
		giftTimerDisplay.SetText(dailyGiftTimer.TimeRemainingStr);
	}

    void updateCoinsDisplay(int newAmount)
    {
        coinsDisplay.updateAmount(newAmount);
    }

    void updateDogFoodDisplay(int newAmount)
    {
        dogFoodDisplay.updateAmount(newAmount);
    }

}
