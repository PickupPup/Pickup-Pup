/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogOutsideSlot : DogSlot
{
	ScoutingDisplay scoutingDisplay;
    Text nameText;
    Text timerText;
	[SerializeField]
	Image dogImageOverride;
    [SerializeField]
    Image redeemableGiftIcon;
    [SerializeField]
    GameObject redeemableGiftDisplay;
	[SerializeField]
	Sprite collarSprite;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		Text[] text = GetComponentsInChildren<Text>();
		nameText = text[0];
		timerText = text[1];
		nameText.text = string.Empty;
		timerText.text = string.Empty;
		scoutingDisplay = GetComponentInParent<ScoutingDisplay>();
	}

	protected override void checkReferences()
	{
		base.checkReferences();
		if(dogImageOverride)
		{
			dogImage = dogImageOverride;
		}
	}

	protected override void handleSceneLoaded(int sceneIndex)
	{
		base.handleSceneLoaded(sceneIndex);
		if(!timerText)
		{
			timerText = gameObject.AddComponent<Text>();
		}
	}
		
	protected override void cleanupReferences()
	{
		base.cleanupReferences();
		if(dog)
		{
			dog.UnsubscribeFromScoutingTimerChange(handleDogTimerChange);
		}
	}

	#endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite)
    {
        base.Init(dog, dogSprite);
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
		checkReferences();
		this.dog = dog;
		this.dogInfo = dog.Info;
		subscribeTimerEvents(dog);
		nameText.text = dog.Name;
		dogImage.sprite = dog.Portrait;
		dog.SetTimer(dogInfo.TimeRemainingScouting);
        timerText.text = dog.TimeRemainingStr;
		dog.ResumeTimer();
	}
		
	public override void ClearSlot()
	{
		// Call Dog functionality first because base method sets dog ref to null:
		dog.StopTimer();
		dogImage.sprite = collarSprite;
		nameText.text = string.Empty;
		timerText.text = string.Empty;
		base.ClearSlot();
	}

	public Dog BringDogIndoors()
	{
		Dog returningDog = this.dog;
		ClearSlot();
		return returningDog;
	}

	void subscribeTimerEvents(Dog dog)
	{
		dog.SubscribeToScoutingTimerChange(handleDogTimerChange);
		dog.SubscribeToScoutingTimerEnd(handleTimerEnd);
		scoutingDisplay.SubscribeToTimerEnd(dog);
	}

	void initDogScouting(Dog dog)
	{
		dog.TrySendToScout();
		subscribeTimerEvents(dog);
	}

	void handleDogTimerChange(Dog dog, float timeRemaining)
	{
		if(timerText)
		{
			timerText.text = dog.RemainingTimeScoutingStr;	
		}
	}

	void handleTimerEnd()
	{
		if(dog)
		{
			// TODO: Should convert to showing the gift panel
		}
	}
		
}
