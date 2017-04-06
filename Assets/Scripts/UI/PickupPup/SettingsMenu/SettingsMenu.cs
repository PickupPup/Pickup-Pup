/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class SettingsMenu : PPUIElement
{
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider sfxToggle;
    [SerializeField]
    Slider navPanelToggle;

    [SerializeField]
    UIElement musicVolume;
    [SerializeField]
    UIElement sfxStatus;
    [SerializeField]
    UIElement navPanelStatus;

    [SerializeField]
    UIButton resetButton;
    [SerializeField]
    UIButton toggleSFXButton;
    [SerializeField]
    UIButton toggleNavPanelButton;

    AudioController audioController;

    public void ResetData()
    {
        if (dataController)
        {
            dataController.Reset();
        }
        requestReloadScene(refreshSystems:true);
    }

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        musicSlider.onValueChanged.AddListener(handleMusicVolumeChange);
        sfxToggle.onValueChanged.AddListener(handleToggleSFXMute);
        navPanelToggle.onValueChanged.AddListener(handleToggleNavPanel);
        toggleSFXButton.SubscribeToClick(toggleSFX);
        toggleNavPanelButton.SubscribeToClick(toggleNavPanel);
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        audioController = AudioController.Instance;
        dataController = PPDataController.GetInstance;
        musicSlider.value = SettingsUtil.GetMusicVolume();
        sfxToggle.value = Global.BoolToInt(!SettingsUtil.SFXMuted);
        navPanelToggle.value = SettingsUtil.NavDropDownType;
    }

    #endregion

    #region UIElement Overrides

    public override void Hide()
    {
        EventController.Event(k.GetPlayEvent(k.BACK));
        base.Hide();
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

    void handleToggleNavPanel(float navPanelStatusf)
    {
        int status = (int) navPanelStatusf;
        if(status != SettingsUtil.NavDropDownType)
        {
            SettingsUtil.ToggleNavDropdownType();
        }
        navPanelStatus.SetText(status == k.STANDARD_DROPDOWN ? k.STANDARD : k.ALTERNATE);
    }

    void toggleSFX()
    {
        bool isMuted = checkSFXMuted(sfxToggle.value);
        isMuted = !isMuted;
        sfxToggle.value = Global.BoolToInt(!isMuted);
    }

    void toggleNavPanel()
    {
        int newStatus;
        if(SettingsUtil.NavDropDownType == k.STANDARD_DROPDOWN)
        {
            newStatus = k.ALT_SINGLE_DROPDOWN;
        }
        else
        {
            newStatus = k.STANDARD_DROPDOWN;
        }
        navPanelToggle.value = newStatus;
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
