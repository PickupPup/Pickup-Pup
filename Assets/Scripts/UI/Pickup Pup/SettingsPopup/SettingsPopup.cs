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

	protected override void setReferences()
	{
		base.setReferences();
		musicSlider.onValueChanged.AddListener(handleMusicVolumeChange);
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		audioController = AudioController.Instance;
	}

	void handleMusicVolumeChange(float newVolume)
	{
		musicVolume.SetText(newVolume.ToString());
		audioController.SetMusicVolume((int) newVolume);
	}

}
