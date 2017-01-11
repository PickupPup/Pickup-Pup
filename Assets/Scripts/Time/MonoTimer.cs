/*
 * Author: Isaiah Mann
 * Description: Time implemented using the MonoBehaviour superclass
 */

using System.Collections;
using UnityEngine;

public class MonoTimer : MonoBehaviourExtended, IGameTimer 
{
	#region Instance Accessors

	public float TimeRemaining
	{
		get;
		private set;
	}

	public bool TimeIsUp 
	{
		get 
		{	
			return TimeRemaining <= 0;
		}
	}

	public bool IsRunning 
	{
		get 
		{
			return timerIsRunning;
		}
	}

	#endregion

	bool timeChangedSinceLastChangeEvent 
	{
		get 
		{
			return timeAtLastChangeEvent != TimeRemaining;
		}
	}

	PPData.DataActionf onTimeChange;
	PPData.DataAction onTimeUp;
	IEnumerator timerCoroutine;
	float timeAtLastChangeEvent = float.MinValue;
	float timeStep;
	float maxTime;
	bool timerIsRunning = false;

	public void Reset() 
	{
		TimeRemaining = maxTime;
		checkForTimeEvents();
	}

	public IGameTimer Setup(float maxTime, float timeStep) 
	{
		this.timeStep = timeStep;
		this.maxTime = maxTime;
		this.TimeRemaining = maxTime;
		DontDestroyOnLoad(gameObject);
		// Hide in the inspector so it doesn't clutter
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		return this;
	}

	public void Teardown() 
	{
		markForDestroyOnLoad();	
	}

	public void Begin(bool resetOnStart) 
	{
		if(resetOnStart) 
		{
			Reset();
		}
		startCoroutine();
	}
		
	public void Resume() 
	{
		if(!timerIsRunning) 
		{
			startCoroutine();
		}
	}
		
	public float Pause() 
	{
		if(timerIsRunning) 
		{
			stopCoroutine();
		}
		return TimeRemaining;
	}

	public float Stop(bool resetOnStop) 
	{
		float timeRemaining = TimeRemaining;
		stopCoroutine();
		if(resetOnStop) 
		{
			Reset();
		}
		return timeRemaining;
	}

	public void SubscribeToTimeChange(PPData.DataActionf action) 
	{
		onTimeChange += action;
	}

	public void UnsubscribeFromTimeChange(PPData.DataActionf action) 
	{
		onTimeChange -= action;
	}

	public void SubscribeToTimeUp(PPData.DataAction action) 
	{
		onTimeUp += action;
	}

	public void UnsubscribeFromTimeUp(PPData.DataAction action) 
	{
		onTimeUp -= action;
	}

	public void ClearEventsOnTimeUp() 
	{
		onTimeChange = null;
		onTimeUp = null;
	}

	public void ModifyTimeRemaining(float deltaTime, bool checkForEvents) 
	{
		this.TimeRemaining += deltaTime;
		if(checkForEvents) 
		{
			this.checkForTimeEvents();
		}
	}

	public void SetTimeRemaining(float newTime, bool checkForEvents) 
	{
		this.TimeRemaining = newTime;
		if(checkForEvents) 
		{
			this.checkForTimeEvents();
		}
	}

	public void ZeroOutTimeRemaining(bool shouldCallTimeUpEvent) 
	{
		TimeRemaining = 0;
		if(shouldCallTimeUpEvent) 
		{
			handleTimeUp();
		}
	}


	void startCoroutine() 
	{
		stopCoroutine();
		timerIsRunning = true;
		timerCoroutine = countdown();
		StartCoroutine(timerCoroutine);
	}

	void stopCoroutine() 
	{
		if(timerCoroutine != null) 
		{
			StopCoroutine(timerCoroutine);
		}
		timerIsRunning = false;
	}

	IEnumerator countdown() 
	{
		while(timerIsRunning) 
		{
			yield return new WaitForSecondsRealtime(timeStep);
			TimeRemaining -= timeStep;
			checkForTimeEvents();
		}
	}

	protected void callOnTimeUp() 
	{
		if(onTimeUp != null) 
		{
			onTimeUp();
		}
	}

	protected void callOnTimeChange(float newTime) 
	{
		if(onTimeChange != null) 
		{
			onTimeChange(newTime);
			timeAtLastChangeEvent = newTime;
		}
	}

	protected void handleTimeChange(float newTime) 
	{
		callOnTimeChange(newTime);
	}

	protected void handleTimeUp() 
	{
		callOnTimeUp();
	}

	protected void checkForTimeEvents() 
	{
		if(TimeIsUp) 
		{
			handleTimeUp();
		}
		if(timeChangedSinceLastChangeEvent) 
		{
			handleTimeChange(TimeRemaining);
		}
	}

}
