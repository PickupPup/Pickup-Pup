using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTest : MonoBehaviourExtended {
	public PPTimer Timer;
	public Text TimeRemaining;

	protected override void SetReferences () {
		base.SetReferences ();
		Timer.Init();
		Timer.SubscribeToTimeChange(updateVisualTimer);
	}

	void updateVisualTimer (float secondsRemaining) {
		TimeRemaining.text = Timer.ToString();
	}

	public void BeginTimer () {
		Timer.Begin();
	}

	public void StopTimer () {
		Timer.Stop();
	}

	public void ResumeTimer () {
		Timer.Resume();
	}

	public void PauseTimer () {
		Timer.Pause();
	}

	public void ResetTimer () {
		Timer.Reset();
	}

	public void AddMinute () {
		Timer.ModifyTimeRemaining(60, checkForEvents:true);
	}
}
