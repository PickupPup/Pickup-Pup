/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that immediately returns a dog from scouting. 
 */

using k = PPGlobal;

public class DogReturnAdReward : AdReward {

    Dog dog;

    public DogReturnAdReward(Dog dog)
    {
        this.dog = dog;
        rewardSFXEvent = k.WHISTLE;
    }

    public override void OfferReward()
    {
        if(dog)
        {
            playRewardSFX();
            dog.ReturnFromScout();
        }
    }
}
