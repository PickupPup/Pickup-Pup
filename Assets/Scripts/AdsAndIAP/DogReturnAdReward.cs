/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that immediately returns a dog from scouting. 
 */

public class DogReturnAdReward : AdReward {

    Dog dog;

    public DogReturnAdReward(Dog dog)
    {
        this.dog = dog;
    }

    public override void offerReward()
    {
        if(dog)
        {
            dog.ReturnFromScout(true);
        }
    }
}
