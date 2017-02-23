/*
 * Author(s): Isaiah Mann
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
        
}
