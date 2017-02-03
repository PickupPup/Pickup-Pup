/*
 * Author: Grace Barrett-Snyder 
 * Description: UI for displaying an individual dog's profile in the Shelter scene
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class DogAdoptProfile : DogProfile
{
    [SerializeField]
    PriceTag priceTag;
    [SerializeField]
    Text adoptButtonText;
    [SerializeField]
    Button adoptButton;

    PPTuning tuning;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        iconsObject.SetActive(false);
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        tuning = gameController.Tuning;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        EventController.Subscribe(handleAdoptEvent);
    }

    #endregion

    #region DogProfile Overrides

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        checkReferences();
        if(checkAdopted(dogInfo))
        {
            showAdopted();
        }
        else
        {
            showDefault();
            priceTag.Set(dogInfo.CostToAdopt);
        }
    }

    #endregion

    void handleAdoptEvent(string eventName, Dog dog)
    {
        if (eventName == k.ADOPT && dog != null && dog.Info.Equals(dogInfo))
        {
            showAdopted();
        }
    }

    bool checkAdopted(DogDescriptor dogInfo)
    {
        return PPDataController.GetInstance.CheckAdopted(dogInfo);
    }

    void showAdopted()
    {
        setComponents(adoptButtonText, tuning.AdoptedText, tuning.AdoptedTextColor, 
            adoptButton.image, tuning.AdoptedBackgroundColor, adoptButton, false, 
            priceTag, false);
    }

    void showDefault()
    {
        bool canAfford = gameController.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt);
        setComponents(adoptButtonText, tuning.AdoptedText, tuning.DefaultTextColor,
            adoptButton.image, tuning.DefaultBackgroundColor, adoptButton, canAfford,
            priceTag, true);
    }

}
