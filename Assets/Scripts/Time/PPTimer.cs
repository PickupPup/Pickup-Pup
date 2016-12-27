/*
 * Author: Isaiah Mann
 * Description: Used to time events in game
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PPTimer : PPData {
	// All time values measured in seconds
	public float TimeStep;
	public float MaxTime;
	[System.NonSerialized]
	// A MonoBehaviour to attach the coroutine to
	IGameTimer timer;
	public bool IsTimerSetup {
		get {
			return timer != null;
		}
	}
	public float TimeRemaining {
		get {
			return timer.TimeRemaining;
		}
	}
	public bool TimeIsUp {
		get {
			return timer.TimeIsUp;
		}
	}
	public bool IsRunning {
		get {
			return timer.IsRunning;
		}
	}

	public PPTimer (float maxTime, float timeStep) {
		this.MaxTime = maxTime;
		this.TimeStep = timeStep;
		setupTimer(maxTime, timeStep);
	}

	public void Init () {
		if (IsTimerSetup) {
			setupTimer(this.MaxTime, this.TimeStep);
		}
	}

	~PPTimer () {
		teardownTimer();
	}

	IGameTimer setupTimer (float maxTime, float timeStep) {
		GameObject timerObject = new GameObject();
		timer = timerObject.AddComponent<MonoTimer>();
		return timer.Setup(maxTime, timeStep);
	}

	void teardownTimer () {
		timer.Teardown();
	}

	public void SubscribeToTimeChange (PPData.DataActionf action) {
		timer.SubscribeToTimeChange(action);
	}

	public void UnsubscribeFromTimeChange (PPData.DataActionf action) {
		timer.UnsubscribeFromTimeChange(action);
	}

	public void SubscribeToTimeUp (DataAction action) {
		timer.SubscribeToTimeUp(action);
	}

	public void UnsubscribeFromTimeUp (DataAction action) {
		timer.UnsubscribeFromTimeUp(action);
	}

	public void ClearEventsOnTimeUp () {
		timer.ClearEventsOnTimeUp();
	}

	public void Reset () {
		timer.Reset();
	}
		
	public void Begin () {
		this.Begin(resetOnBegin:true);
	}

	public void Begin (bool resetOnBegin) {
		timer.Begin(resetOnBegin);
	}

	public float Pause () {
		return timer.Pause();
	}

	public void Resume () {
		timer.Resume();
	}

	public void Stop () {
		this.Stop(resetOnStop:true);
	}
		
	public float Stop (bool resetOnStop) {
		return timer.Stop(resetOnStop);
	}

	public void ModifyTimeRemaining (float deltaTime, bool checkForEvents) {
		timer.ModifyTimeRemaining(deltaTime, checkForEvents);
	}

	public void SetTimeRemaining (float newTime, bool checkForEvents) {
		timer.ModifyTimeRemaining(newTime, checkForEvents);
	}

	public void ZeroOutTimeRemaining (bool shouldCallTimeUpEvent) {
		timer.ZeroOutTimeRemaining(shouldCallTimeUpEvent);
	}

	// Returns time in HH:MM:SS format
	public override string ToString () {
		int hours = (int) TimeRemaining / 3600;
		int minutes = ((int) TimeRemaining / 60) % 60;
		int seconds = (int) TimeRemaining % 60;
		return string.Format("{0}:{1}:{2}", 
			padWithZeroes(hours, 2), 
			padWithZeroes(minutes, 2),
			padWithZeroes(seconds, 2));
	}
}
