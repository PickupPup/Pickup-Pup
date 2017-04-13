/*
 * Author: Isaiah Mann
 * Description: Time implemented using the MonoBehaviour superclass
 */

using System.Collections;
using UnityEngine;

using k = PPGlobal;

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

    float scaledTimeStep
    {
        get
        {
            if(gameController)
            {
                return this.timeStep * gameController.TimeScale;
            }
            else
            {
                return this.timeStep * k.DEFAULT_TIME_SCALE;
            }
        }
    }

    PPData.DataAction onTimeBegin;
	PPData.DataActionf onTimeChange;
	PPData.DataAction onTimeUp;
	IEnumerator timerCoroutine;
	float timeAtLastChangeEvent = float.MinValue;
	float timeStep;
	float maxTime;
	bool timerIsRunning = false;

	#region MonoBehaviourExtended Overrides 

	public override bool TryUnsubscribeAll()
	{
		base.TryUnsubscribeAll();
        onTimeBegin = null;
		onTimeChange = null;
		onTimeUp = null;
		return true;
	}

	#endregion

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
        handleTimeBegin();
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

    public void SubscribeToTimeBegin(PPData.DataAction action)
    {
        onTimeBegin += action;
    }

    public void UnsubscribeFromTimeBegin(PPData.DataAction action)
    {
        onTimeBegin -= action;
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
        onTimeBegin = null;
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
            TimeRemaining -= scaledTimeStep;
			checkForTimeEvents();
		}
	}

    protected void callOnTimeBegin()
    {
        if(onTimeBegin != null)
        {
            onTimeBegin();
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

    protected void handleTimeBegin()
    {
        callOnTimeBegin();
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
