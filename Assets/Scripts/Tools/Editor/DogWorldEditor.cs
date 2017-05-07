/*
 * Author(s): Isaiah Mann
 * Description: Extends the editor options for the DogWorld option
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DogWorld))]
public class DogWorldEditor : Editor
{
	const string ENABLE_WANDERING = "Enable Wandering";
	const string DISABLE_WANDERING = "Disable Wandering";

	string buttonStateText
	{
		get
		{
			DogWorld world = (DogWorld) target;
			if(world.WanderingIsEnabled)
			{
				return DISABLE_WANDERING;
			}
			else
			{
				return ENABLE_WANDERING;
			}
		}
	}

	#region Editor Overrides

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DogWorld world = (DogWorld) target;
		if(GUILayout.Button(buttonStateText))
		{
			world.ToggleWanderingEnabled(!world.WanderingIsEnabled);
		}
	}

	#endregion

}
