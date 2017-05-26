/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching ads. 
 */

using k = PPGlobal;

public abstract class AdReward
{
    protected string rewardSFXEvent;

    public abstract void OfferReward();

    protected void playRewardSFX()
    {
        EventController.Event(k.GetPlayEvent(rewardSFXEvent));
    }

}
