/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PPAnalyticsController))]
public class PPAnalyticsControllerEditor : Editor
{
	const string ENABLE_SEND_IN_EDITOR = "Enable Send in Editor";
	const string DISABLE_SEND_IN_EDITOR = "Disable Send in Editor";

	string buttonText
	{
		get
		{
			if(Mixpanel.SendInEditor)
			{
				return DISABLE_SEND_IN_EDITOR;
			}
			else
			{
				return ENABLE_SEND_IN_EDITOR;
			}
		}
	}

	#region Editor Overrides

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if(GUILayout.Button(buttonText))
		{
			Mixpanel.ToggleSendInEditor(!Mixpanel.SendInEditor);
		}
	}

	#endregion

}
