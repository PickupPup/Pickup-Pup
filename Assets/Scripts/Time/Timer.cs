/*
 * Author: Isaiah Mann
 * Description: Used to time events in game
 */

using System.Collections;
using UnityEngine;

[System.Serializable]
public class Timer : PPData {
	// All Time measured in seconds
	public float TimeStep;

	IEnumerator countdown () {
		yield return new WaitForSecondsRealtime(TimeStep);
	}
}
