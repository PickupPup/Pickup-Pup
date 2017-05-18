/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that gives the player coins. 
 */

public class CoinsAdReward : AdReward {

    int amount = 100; // Set this in tuning
    PPGameController gameController;

    public CoinsAdReward(int amount = 100)
    {
        this.amount = amount;
        gameController = PPGameController.GetInstance;
    }

    public override void offerReward()
    {
        gameController.ChangeCoins(amount);
    }

}
