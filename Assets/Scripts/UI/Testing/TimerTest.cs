/*
 * Author: Isaiah Mann
 * Description: Testing timer scripts
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTest : MonoBehaviourExtended 
{
	[SerializeField]
	PPTimer timer;
	[SerializeField]
	Text timeRemaining;

	#region MonoBehaviourExtended Overrides 

	protected override void setReferences() 
	{
		base.setReferences();
		timer.Init();
		timer.SubscribeToTimeChange(updateVisualTimer);
	}

	#endregion

	void updateVisualTimer(float secondsRemaining) 
	{
		timeRemaining.text = timer.ToString();
	}

	public void BeginTimer() 
	{
		timer.Begin();
	}

	public void StopTimer() 
	{
		timer.Stop();
	}

	public void ResumeTimer() 
	{
		timer.Resume();
	}

	public void PauseTimer() 
	{
		timer.Pause();
	}

	public void ResetTimer() 
	{
		timer.Reset();
	}

	public void AddMinute() 
	{
		timer.ModifyTimeRemaining(60, checkForEvents:true);
	}

}
