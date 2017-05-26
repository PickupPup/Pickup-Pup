/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that gives the player coins. 
 */

using k = PPGlobal;

public class CoinsAdReward : AdReward
{
    PPGameController gameController;
    int amount;

    public CoinsAdReward()
    {
        gameController = PPGameController.GetInstance;
        PPTuning tuning = gameController.Tuning;
        this.amount = tuning.VideoAdCoinBonus;
        rewardSFXEvent = k.GIFT_REDEEM;
    }

    public override void OfferReward()
    {
        playRewardSFX();
        gameController.ChangeCoins(amount);
    }

}
