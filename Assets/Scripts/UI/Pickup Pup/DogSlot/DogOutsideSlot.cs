/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogOutsideSlot : DogSlot
{
    Text nameText;
    Text timerText;

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);
        Text[] text = GetComponentsInChildren<Text>();
        nameText = text[0];
        timerText = text[1];

        nameText.text = dog.Name;
    }

	public override void Init(Dog dog)
	{
		base.Init(dog);
		initDogScouting(dog);
	}

    #endregion

	public Dog BringDogIndoors()
	{
		Dog returningDog = this.dog;
		ClearSlot();
		return returningDog;
	}

	void initDogScouting(Dog dog)
	{
		dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
		dog.TrySendToScout();
	}

	void handleDogTimerChange(Dog dog, float timeRemaining)
	{
		timerText.text = dog.RemainingTimeScoutingStr;	
	}
		
}
