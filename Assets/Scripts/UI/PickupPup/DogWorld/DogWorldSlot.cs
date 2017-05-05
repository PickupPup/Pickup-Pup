/*
 * Authors: Isaiah Mann, Ben Page, Grace Barrett-Snyder
 * Description: Dogs will travel / be displayed at this spot on the UI
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

public class DogWorldSlot : DogSlot
{
    [SerializeField]
    NameTag nameTag;

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        UISFXHandler sfxScript = GetComponent<UISFXHandler>();
        sfxScript.DisableSounds();
        GetComponent<Button>().transition = Selectable.Transition.None;       
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

    public override void Init(DogDescriptor dog)
    {
        base.Init(dog);
        if(nameTag)
        {
            nameTag.Init(this, this.dog);
        }
        checkReferences();
    }

    protected override void setSprite (DogDescriptor dog)
    {
        this.dogInfo = dog;
        dogImage.sprite = dog.WorldSprite;
        dog.SubscribeToBeginScouting(handleScoutingBegun);
        dog.SubscribeToDoneScouting(handleScoutingDone);
    }

    #endregion

    public void SubscribeToNameTagClick(PPData.DogAction clickAction)
    {
        if(nameTag)
        {
            nameTag.SubscribeToClick(clickAction);
        }
    }

    public void UnsubscribeFromNameTagClick(PPData.DogAction clickAction)
    {
        if(nameTag)
        {
            nameTag.UnsubscribeFromClick(clickAction);
        }
    }

    public override void ExecuteClick()
    {
        base.ExecuteClick();
        moveToFront();
    }

    public void Deselect()
    {
        if(nameTag)
        {
            nameTag.TryDeselect();
        }
    }

    // Hide dog on scouting begun
    void handleScoutingBegun()
    {
        this.Hide();
    }

    void handleScoutingDone()
    {
        this.Show();
    }

    void moveToFront()
    {
        transform.SetAsLastSibling();
    }

}
