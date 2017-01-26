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
	[SerializeField]
	UIButton collectGiftButton;
	[SerializeField]
	GameObject giftTimerActiveDisplay;
	[SerializeField]
	GameObject giftAvailableDisplay;
	[Space(10)]
	[SerializeField]
	GameObject giftReportPrefab;
	[Space(10)]
	[SerializeField]
	bool overrideTimerForDebugging = false;

	PPTimer dailyGiftTimer;
    PPDataController dataController;
	PPGiftController giftController;

    #region MonoBehaviourExtended Overrides

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
		if(dailyGiftTimer != null)
		{
			dailyGiftTimer.UnsubscribeFromTimeChange(handleDailyGiftCountDownChange);
			dailyGiftTimer.UnsubscribeFromTimeUp(makeDailyGiftAvailableToRedeem);
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
        if(dataController.HasGiftToRedeem)
        {
            makeDailyGiftAvailableToRedeem();
        }
        else
        {
            startGiftTimer();
        }
	}
		
	void startGiftTimer()
	{
		toggleBetweenTimerAndGiftReceived(giftReceived:false);
		resetAndBeginGiftTimer();
	}
		
	void resetAndBeginGiftTimer()
	{
        collectGiftButton.TryUnsubscribeAll();
		(dailyGiftTimer as ISubscribable).TryUnsubscribeAll();
		dailyGiftTimer.SubscribeToTimeChange(handleDailyGiftCountDownChange);
		dailyGiftTimer.SubscribeToTimeUp(makeDailyGiftAvailableToRedeem);
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

	void makeDailyGiftAvailableToRedeem()
	{
		toggleBetweenTimerAndGiftReceived(giftReceived:true);
		collectGiftButton.TryUnsubscribeAll();
		collectGiftButton.SubscribeToClick(redeemReceivedGift);
        dataController.NotifyHasGiftToRedeem();
	}

	void toggleBetweenTimerAndGiftReceived(bool giftReceived)
	{
		bool timerActive = !giftReceived;
		giftTimerActiveDisplay.SetActive(timerActive);
		giftAvailableDisplay.SetActive(giftReceived);
		if(timerActive)
		{
			resetAndBeginGiftTimer();
		}
	}

	void redeemReceivedGift()
	{
		CurrencyData gift = getDailyGift();
        dataController.RedeemGift(gift);
        UIElement giftReport;
        if(!UIElement.TryPullFromSpawnPool(typeof(GiftReportUI), out giftReport))
        {
            giftReport = Instantiate(giftReportPrefab).GetComponent<GiftReportUI>();
        }
		(giftReport as GiftReportUI).Init(gift);
		toggleBetweenTimerAndGiftReceived(giftReceived:false);
	}

}
