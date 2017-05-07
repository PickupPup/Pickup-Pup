/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the display of multiple currencies on the Currency Panel
 */

using System;
using UnityEngine;
using k = PPGlobal;

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
	PPGiftController giftController;

	DateTime timePaused = default(DateTime);

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

	protected override void handleGameTogglePause(bool isPaused)
	{
		base.handleGameTogglePause(isPaused);
		if(isPaused)
		{
			this.timePaused = DateTime.Now;
		}
		else
		{
			if(dailyGiftTimer != null && dailyGiftTimer.IsRunning && hasTimePaused())
			{
				double secondsPassed = (DateTime.Now - this.timePaused).TotalSeconds;
				float updatedTimeRemaining = dailyGiftTimer.TimeRemaining - (float) secondsPassed;
				updatedTimeRemaining = Mathf.Clamp(updatedTimeRemaining, k.NONE_VALUE, float.MaxValue);
				if(updatedTimeRemaining > k.NONE_VALUE)
				{
					dailyGiftTimer.SetTimeRemaining(updatedTimeRemaining, checkForEvents:false);
				}
				else 
				{
					dailyGiftTimer.ZeroOutTimeRemaining(shouldCallTimeUpEvent:true);
				}
				this.timePaused = default(DateTime);
			}
		}
	}

    #endregion

	public void Init(PPGameController gameController, PPDataController dataController, PPGiftController giftController)
    {
        unsubscribeEvents();
		this.giftController = giftController;
        this.dataController = dataController;
        subscribeEvents();

        // Display Updated Currency
        dogFoodDisplay.Init(dataController.DogFood, dataController);
        coinsDisplay.Init(dataController.Coins, dataController);
        initDailyGiftCountdown(gameController.Tuning, dataController);
    }

    CurrencyData getDailyGift() 
    {
		return giftController.GetDailyGift();
    }

    void initDailyGiftCountdown(PPTuning tuning, PPDataController dataController)
	{
		float dailyGiftCountdown;
        bool redeemOnTimerReset = false;
		if(dataController.DailyGiftCountdownRunning && !overrideTimerForDebugging)
		{
			dailyGiftCountdown = dataController.DailyGiftCountdown;
		}
		else
		{
            dailyGiftCountdown = tuning.WaitTimeSecsForDailyGift;
            redeemOnTimerReset = true;
		}
		dailyGiftTimer = new PPTimer(dailyGiftCountdown, tuning.DefaultTimerTimeStepSec);
        giftTimerDisplay.SetText(dailyGiftTimer.TimeRemainingStr);
        if(dataController.HasGiftToRedeem || redeemOnTimerReset)
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
		resetAndBeginGiftTimer();
	}
		
	void resetAndBeginGiftTimer()
	{
        collectGiftButton.ToggleInteractable(false);
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
        toggleBetweenTimerAndGiftReceived(giftReceived: true);
        collectGiftButton.TryUnsubscribeAll();
        collectGiftButton.ToggleInteractable(true);
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

	bool hasTimePaused()
	{
		return this.timePaused != default(DateTime);
	}

}
