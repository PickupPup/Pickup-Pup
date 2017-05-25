/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that gives the player coins. 
 */

public class CoinsAdReward : AdReward
{
    PPGameController gameController;
    int amount;

    public CoinsAdReward(int amount = 100)
    {
        gameController = PPGameController.GetInstance;
        PPTuning tuning = gameController.Tuning;
        this.amount = tuning.VideoAdCoinBonus;
    }

    public override void OfferReward()
    {
        gameController.ChangeCoins(amount);
    }

}
