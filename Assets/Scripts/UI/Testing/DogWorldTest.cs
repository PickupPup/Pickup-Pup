/*
 * Author(s): Isaiah Mann
 * Description: Tests showing dogs w/ the dog world feature
 * Usage: [no notes]
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogWorldTest : MonoTest 
{
    [SerializeField]
    GameObject dogProfileObject;
    DogProfile dogProfile;
    DogWorldSlot[] dogWorldSlots;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        dogWorldSlots = GetComponentsInChildren<DogWorldSlot>();
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        foreach(DogWorldSlot dogSlot in dogWorldSlots)
        {
            dogSlot.SubscribeToNameTagClick(handleNameTagClicked);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        foreach(DogWorldSlot dogSlot in dogWorldSlots)
        {
            dogSlot.UnsubscribeFromNameTagClick(handleNameTagClicked);
        }
    }

    #endregion

    public override bool RunTest(out string feedback)
    {
        DogDatabase data = DogDatabase.GetInstance;
        for(int i = 0; i < 1; i++)
        {
            dataController.Adopt(data.Dogs[i]);
            dogWorldSlots[i].Init(dataController.AdoptedDogs[i]);
        }
        feedback = "Good job, Joel";
        return true;
    }

    void handleNameTagClicked(Dog dog)
    {
        showProfile(dog);   
    }

    void showProfile(Dog dog)
    {
        if(!dogProfile && dogProfileObject)
        {
            dogProfile = dogProfileObject.GetComponent<DogProfile>();
        }
        if(dogProfile)
        {
            dogProfile.Show();
        }
    }

}
