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

	#region MonoBehaviourExtended Overrides

	protected override void setReferences ()
	{
		base.setReferences ();
		Text[] text = GetComponentsInChildren<Text>();
		nameText = text[0];
		timerText = text[1];
		nameText.text = string.Empty;
		timerText.text = string.Empty;
	}

	#endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);
        nameText.text = dog.Name;
    }
		
	public override void Init(Dog dog)
	{
		base.Init(dog);
		initDogScouting(dog);
	}

    #endregion

	public void ResumeScouting(Dog dog)
	{
		this.dog = dog;
		this.dogInfo = dog.Info;
		dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
		nameText.text = dog.name;
		dog.SetTimer(dogInfo.TimeRemainingScouting);
		dog.ResumeTimer();
	}

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
