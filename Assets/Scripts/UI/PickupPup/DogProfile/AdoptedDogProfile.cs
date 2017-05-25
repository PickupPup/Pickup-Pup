/*
 * Author(s): Isaiah Mann
 * Description: Describes special behaviour only related to adopted dogs
 * Usage: [no notes]
 */

using UnityEngine;

public class AdoptedDogProfile : DogProfile 
{
    [SerializeField]
    SouvenirDisplay displaySouvenirPrefab;

    [SerializeField]
    ScoutingButton scoutingButton;
    [SerializeField]
    PPUIButton souvenirButton;

    [SerializeField]
    AffectionMeter affectionMeter;

    SouvenirDisplay activeSouvenirDisplay = null;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        souvenirButton.SubscribeToClick(showSouvenir);
    }
   
    #endregion

    #region DogProfile Overrides

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        affectionMeter.setAffection(dog.Info.Affection);
        souvenirButton.ToggleEnabled(dog.Info.SouvenirCollected);
        scoutingButton.SetIcon(dog);
    }

    #endregion

    void showSouvenir()
    {
        // Don't show extra copies of the souvenir
        if(activeSouvenirDisplay)
        {
            return;
        }

        SouvenirDisplay display = spawnSouvenirDisplay();
        display.Init(dogInfo.Souvenir);
    }

    SouvenirDisplay spawnSouvenirDisplay()
    {
        UIElement elem;
        if(UIElement.TryPullFromSpawnPool(typeof(SouvenirDisplay), out elem))
        {
            return elem as SouvenirDisplay;
        }
        else
        {
            return Instantiate(displaySouvenirPrefab);
        }
    }

}
