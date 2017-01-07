/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogOutsideSlot : DogSlot
{
	Dog outdoorDog;
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

    #endregion

	public void SendDogOutside(Dog dog)
	{
		Init(dog.Info, dog.Portrait);

	}

	public Dog BringDogIndoors()
	{
		Dog returningDog = this.outdoorDog;
		this.outdoorDog = null;
		return returningDog;
	}

	void subscribeToTimer(Dog dog)
	{
		this.outdoorDog = dog;
	}

	void handleDogTimerChange(Dog dog, float timeRemaining)
	{
		
	}
		
}
