/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot in the Shelter scene (has price tag).
 */

using UnityEngine;
using k = PPGlobal;

public class DogShelterSlot : DogSlot
{
    PriceTag priceTag;
    PPTuning tuning;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        priceTag = GetComponentInChildren<PriceTag>();
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

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog)
    {
        base.Init(dog);

        checkReferences();
        priceTag.Set(dog.CostToAdopt);
        priceTag.SetNonPurchasable(tuning.AdoptedText, tuning.NonPurchasableTextColor, tuning.NonPurchasableBackgroundColor);

        if (checkAdopted())
        {
            ShowAdopt();
        }
        else
        {
            ShowDefault();
        }
    }

    #endregion

    bool checkAdopted()
    {
        return PPDataController.GetInstance.CheckIsAdopted(dogInfo);
    }

    void handleAdoptEvent(string eventName, Dog dog)
    {
        if (eventName == k.ADOPT && dog != null && dog.Info.Equals(dogInfo))
        {
            ShowAdopt();
        }
    }

    public void ShowAdopt()
    {
        priceTag.ShowNonPurchasable();
    }

    public void ShowDefault()
    {
        priceTag.ShowPurchasable();
    }

}
