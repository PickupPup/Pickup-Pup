/*
 * Author: Isaiah Mann
 * Description: Used to time events in game
 */

using System.Collections;
using UnityEngine;

[System.Serializable]
public class Timer : PPData {
	// All time values measured in seconds
	public float TimeStep;
	public float TimeRemaining {get; private set;}
	public float MaxTime;
	protected DataAction onTimeUp;
	IEnumerator timerCoroutine;
	// Need a MonoBehaviour to attack a coroutine to
	GameObject timerObject;

	public Timer (float maxTime, float timeStep) {
		this.MaxTime = maxTime;
		this.TimeRemaining = maxTime;
		this.TimeStep = timeStep;
	}

	public bool TimeIsUp {
		get {
			return TimeRemaining <= 0;
		}
	}
		
	public void SubscribeToTimeUp (DataAction action) {
		onTimeUp += action;
	}

	public void UnsubscribeFromTimeUp (DataAction action) {
		onTimeUp -= action;
	}

	public void Reset () {
		this.TimeRemaining = this.MaxTime;
	}

	void startCoroutine () {
		timerCoroutine.MoveNext();
	}

	void stopCoroutine () {
		if (timerCoroutine != null) {
			timerCoroutine.Reset();
		}
	}

	public void Start (bool resetOnStart) {
		if (resetOnStart) {
			Reset();
		}

	}

//	public float Pause () {
//		//
//	}
//
//	public float Stop (bool resetOnStop) {
//
//	}

	public void ModifyTimeRemaining (float deltaTime, bool checkForEvents) {
		this.TimeRemaining += deltaTime;
		if (checkForEvents) {
			this.checkForTimeEvents();
		}
	}

	public void SetTimeRemaining (float newTime, bool checkForEvents) {
		this.TimeRemaining = newTime;
		if (checkForEvents) {
			this.checkForTimeEvents();
		}
	}

	public void ZeroOutTimeRemaining (bool shouldCallTimeUpEvent) {
		TimeRemaining = 0;
		if (shouldCallTimeUpEvent) {
			handleTimeUp();
		}
	}

	// Returns time in HH:MM:SS format
	public override string ToString () {
		int hours = (int) TimeRemaining / 3600;
		int minutes = ((int) TimeRemaining / 60) % 60;
		int seconds = (int) TimeRemaining % 60;
		return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
	}
		
	protected void callOnTimeUp () {
		if (onTimeUp != null) {
			onTimeUp();
		}
	}

	protected void handleTimeUp () {
		callOnTimeUp();
	}

	protected void checkForTimeEvents () {
		if (TimeIsUp) {
			handleTimeUp();
		}
	}

	IEnumerator countdown () {
		yield return new WaitForSecondsRealtime(TimeStep);
		TimeRemaining -= TimeStep;
		if (TimeIsUp) {
			handleTimeUp();
		}
	}
}
