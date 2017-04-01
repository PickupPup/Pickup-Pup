/*
 * Author: Isaiah Mann
 * Description: Used to time events in game
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PPTimer : PPData, ISubscribable
{
	#region Static Accessors

	public static PPTimer DefaultTimer
	{
		get 
		{
			return new PPTimer(NONE_INT, NONE_INT);
		}
	}

	public static string DefaultTimeRemainingStr
	{
		get 
		{
			return DefaultTimer.TimeRemainingStr;
		}
	}

	#endregion

	#region Instance Accessors

	public bool IsTimerSetup 
	{
		get 
		{
			return timer != null;
		}
	}

	public float TimeRemaining 
	{
		get 
		{
			return timer.TimeRemaining;
		}
	}

	public string TimeRemainingStr
	{
		get 
		{
			return this.ToString();
		}
	}

	public bool TimeIsUp 
	{
		get 
		{
			return timer.TimeIsUp;
		}
	}

	public bool IsRunning 
	{
		get 
		{
			return timer.IsRunning;
		}
	}

	#endregion

	// All time values measured in seconds
	[SerializeField]
	float timeStep;
	[SerializeField]
	float maxTime;

	// A MonoBehaviour to attach the coroutine to
	[System.NonSerialized]
	IGameTimer timer;

    public PPTimer(float maxTime, float timeStep, bool autoStopAtZero = true) 
	{
		this.maxTime = maxTime;
		this.timeStep = timeStep;
        this.timer = setupTimer(maxTime, timeStep);
        if(autoStopAtZero)
        {
            this.timer.SubscribeToTimeUp(Stop);
            this.timer.SubscribeToTimeUp(delegate 
                {
                    SetTimeRemaining(0, checkForEvents:false);
                });
        }
	}

	public void Init() 
	{
		if(IsTimerSetup) 
		{
			setupTimer(this.maxTime, this.timeStep);
		}
	}

	~PPTimer() 
	{
		teardownTimer();
	}

	IGameTimer setupTimer(float maxTime, float timeStep) 
	{
		GameObject timerObject = new GameObject();
		MonoTimer timer = timerObject.AddComponent<MonoTimer>();
		return timer.Setup(maxTime, timeStep);
	}

	void teardownTimer() 
	{
		timer.Teardown();
	}

	#region ISubscribable Interface

	bool ISubscribable.TryUnsubscribeAll()
	{
		return timer.TryUnsubscribeAll();
	}

    #endregion

    public void SubscribeToTimeBegin(DataAction action)
    {
        timer.SubscribeToTimeBegin(action);
    }

    public void UnsubscribeFromTimeBegin(DataAction action)
    {
        timer.UnsubscribeFromTimeBegin(action);
    }

    public void SubscribeToTimeChange(PPData.DataActionf action) 
	{
		timer.SubscribeToTimeChange(action);
	}

	public void UnsubscribeFromTimeChange(PPData.DataActionf action) 
	{
		timer.UnsubscribeFromTimeChange(action);
	}

	public void SubscribeToTimeUp(DataAction action) 
	{
		timer.SubscribeToTimeUp(action);
	}

	public void UnsubscribeFromTimeUp(DataAction action) 
	{
		timer.UnsubscribeFromTimeUp(action);
	}

	public void ClearEventsOnTimeUp() 
	{
		timer.ClearEventsOnTimeUp();
	}

	public void Reset() 
	{
		timer.Reset();
	}
		
	public void Begin() 
	{
		this.Begin(resetOnBegin:true);
	}

	public void Begin(bool resetOnBegin) 
	{
		timer.Begin(resetOnBegin);
	}

	public float Pause() 
	{
		return timer.Pause();
	}

	public void Resume() 
	{
		timer.Resume();
	}

	public void Stop() 
	{
		this.Stop(resetOnStop:true);
	}
		
	public float Stop(bool resetOnStop) 
	{
		return timer.Stop(resetOnStop);
	}

	public void ModifyTimeRemaining(float deltaTime, bool checkForEvents) 
	{
		timer.ModifyTimeRemaining(deltaTime, checkForEvents);
	}

	public void SetTimeRemaining(float newTime, bool checkForEvents) 
	{
		timer.SetTimeRemaining(newTime, checkForEvents);
	}

	public void ZeroOutTimeRemaining(bool shouldCallTimeUpEvent) 
	{
		timer.ZeroOutTimeRemaining(shouldCallTimeUpEvent);
	}

	#region System.Objrect Overrides

	// Returns time in HH:MM:SS format
	public override string ToString() 
	{
		return formatTime(TimeRemaining);
	}

	#endregion

}
