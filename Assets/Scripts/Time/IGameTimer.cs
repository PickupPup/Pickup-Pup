﻿/*
 * Author: Isaiah Mann
 * Description: Unity Independent setup for a timer
 */

public interface IGameTimer {
	bool TimeIsUp {get;}
	bool IsRunning {get;}
	float TimeRemaining {get;}

	IGameTimer Setup (float timeStep, float maxTime);
	void Teardown();
	void Reset();
	void Begin(bool resetOnStart);
	void Resume();
	float Pause();
	float Stop(bool resetOnStop);
	void SubscribeToTimeChange (PPData.DataActionf action);
	void UnsubscribeFromTimeChange (PPData.DataActionf action);
	void SubscribeToTimeUp (PPData.DataAction action);
	void UnsubscribeFromTimeUp (PPData.DataAction action);
	void ClearEventsOnTimeUp();
	void ModifyTimeRemaining (float deltaTime, bool checkForEvents);
	void SetTimeRemaining (float newTime, bool checkForEvents);
	void ZeroOutTimeRemaining (bool shouldCallTimeUpEvent);
}