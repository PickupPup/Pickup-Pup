/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : PPUIElement 
{
	[SerializeField]
	Slider musicSlider;
	[SerializeField]
	Slider sfxToggle;

	[SerializeField]
	UIElement musicVolume;
	[SerializeField]
	UIElement sfxStatus;

	[SerializeField]
	UIButton resetButton;

	AudioController audioController;
	PPDataController dataController;

	public void ResetData()
	{
		if(dataController)
		{
			dataController.Reset();
		}
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		musicSlider.onValueChanged.AddListener(handleMusicVolumeChange);
		sfxToggle.onValueChanged.AddListener(handleToggleSFXMute);
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		audioController = AudioController.Instance;
		dataController = PPDataController.GetInstance;
		musicSlider.value = SettingsUtil.GetMusicVolume();
		sfxToggle.value = Global.BoolToInt(!SettingsUtil.SFXMuted);
	}

	#endregion

	// Limitation of the Toggle Slider: uses float values
	// 0 == false, 1 == true
	void handleToggleSFXMute(float sfxMuteState)
	{
		bool isMuted = checkSFXMuted(sfxMuteState);
		SettingsUtil.ToggleSFXMuted(isMuted);
		// TODO: Override w/ Language Dict text:
		sfxStatus.SetText(isMuted ? OFF : ON);
	}
		
	bool checkSFXMuted(float sfxMuteState)
	{
		int state = (int) sfxMuteState;
		return !Global.IntToBool(state);
	}

	void handleMusicVolumeChange(float newVolume)
	{
		musicVolume.SetText(newVolume.ToString());
		audioController.SetMusicVolume((int) newVolume);
	}

}
