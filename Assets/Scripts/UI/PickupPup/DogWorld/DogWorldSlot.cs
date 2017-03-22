/*
 * Author(s): Isaiah Mann, Ben Page
 * Description: Dogs will travel / be displayed at this spot on the UI
 * Usage: [no notes]
 */

using UnityEngine.UI;

public class DogWorldSlot : DogSlot
{	
    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        dogImage = GetComponent<Image>();
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        if(dogInfo != null)
        {
            dogInfo.UnsubscribeFromBeginScouting(handleScoutingBegun);
            dogInfo.UnsubscribeFromDoneScouting(handleScoutingDone);
        }
    }

    #endregion

    #region DogSlot Overrides 

    protected override void setSprite (DogDescriptor dog)
    {
        this.dogInfo = dog;
        dogImage.sprite = dog.WorldSprite;
        dog.SubscribeToBeginScouting(handleScoutingBegun);
        dog.SubscribeToDoneScouting(handleScoutingDone);
    }

    #endregion

    // Hide dog on scouting begun
    void handleScoutingBegun()
    {
        this.Hide();
    }

    void handleScoutingDone()
    {
        this.Show();
    }

    /* BP Unsure if this is the preferred spot for this, but we apparently
     * don't want any click noises for the dogs in the scene (yard/living room) */
    void Start()
    {
        UISFXHandler sfxScript = GetComponent<UISFXHandler>();
        sfxScript.clickEnabledSoundEvent = "";
        sfxScript.clickDisabledSoundEvent = "";

    }

}
