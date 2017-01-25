﻿/*
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
	[SerializeField]
	GameObject giftReportPrefab;
	[SerializeField]
	bool overrideTimerForDebugging = false;

	PPTimer dailyGiftTimer;
    PPDataController dataController;
	PPGiftController giftController;

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
		if(dailyGiftTimer != null)
		{
			dailyGiftTimer.UnsubscribeFromTimeChange(handleDailyGiftCountDownChange);
			dailyGiftTimer.UnsubscribeFromTimeUp(receiveDailyGift);
		}
    }

    #endregion

	public void Init(PPGameController gameController, PPDataController dataController, PPGiftController giftController)
    {
        unsubscribeEvents();
        this.dataController = dataController;
		this.giftController = giftController;
        subscribeEvents();

        // Display Updated Currency
        dogFoodDisplay.Init(dataController, dataController.DogFood);
        coinsDisplay.Init(dataController, dataController.Coins);
		initDailyGiftCountdown(gameController.Tuning, dataController);
    }

    CurrencyData getDailyGift() 
    {
		return giftController.GetDailyGift();
    }

	void initDailyGiftCountdown(PPTuning tuning, PPDataController dataController)
	{
		float dailyGiftCountdown;
		if(dataController.DailyGiftCountdownRunning && !overrideTimerForDebugging)
		{
			dailyGiftCountdown = dataController.DailyGiftCountdown;
		}
		else
		{
            dailyGiftCountdown = tuning.WaitTimeSecsForDailyGift;
		}
		dailyGiftTimer = new PPTimer(dailyGiftCountdown, tuning.DefaultTimerTimeStepSec);
		dailyGiftTimer.SubscribeToTimeChange(handleDailyGiftCountDownChange);
		dailyGiftTimer.SubscribeToTimeUp(receiveDailyGift);
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

	void receiveDailyGift()
	{
		CurrencyData gift = getDailyGift();
		dataController.GiveCurrency(gift);
		displayReceivedGift(gift);
	}

	void displayReceivedGift(CurrencyData gift)
	{
        UIElement giftReport;
        if(!UIElement.TryPullFromSpawnPool(typeof(GiftReportUI), out giftReport))
        {
            giftReport = Instantiate(giftReportPrefab).GetComponent<GiftReportUI>();
        }
        (giftReport as GiftReportUI).Init(gift);
	}

}
