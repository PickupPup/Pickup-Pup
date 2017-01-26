/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot in the Shelter scene (has price tag).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogAdoptionSlot : DogSlot
{
    [SerializeField]
    UIElement iconHolder;
    [SerializeField]
    Image priceBackgroundImage;
    Text priceOrAdoptionStatus;

    PPDataController dataController;
    PPTuning tuning;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        priceOrAdoptionStatus = GetComponentInChildren<Text>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
        tuning = game.Tuning;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if(dataController)
        {
			dataController.SubscribeToCurrencyChange(CurrencyType.Coins, updateTextColor);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if(dataController)
        {
			dataController.UnsubscribeFromCurrencyChange(CurrencyType.Coins, updateTextColor);
        }
    }

    #endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite)
    {
        unsubscribeEvents();
        base.Init(dog, dogSprite);
        subscribeEvents();
        
        checkReferences();
        if(checkAdopted())
        {
            ShowAdopt();
        }
        else
        {
            ShowDefault();
            updateTextColor(game.Coins.Amount);
        }
    }

    #endregion

    bool checkAdopted()
    {
        return PPDataController.GetInstance.CheckAdopted(dogInfo);
    }

    public void ShowAdopt()
    {
        setComponents(tuning.AdoptedText, tuning.AdoptedTextColor, tuning.AdoptedBackgroundColor, false);
        unsubscribeEvents();
    }

    public void ShowDefault()
    {
        setComponents(dogInfo.CostToAdoptStr, tuning.DefaultTextColor, tuning.DefaultBackgroundColor, true);
    }

    void setComponents(string priceOrAdoptionText, Color priceOrAdoptionTextColor, 
        Color priceBackgroundColor, bool showIconHolder)
    {
        priceOrAdoptionStatus.text = priceOrAdoptionText;
        priceOrAdoptionStatus.color = priceOrAdoptionTextColor;
        priceBackgroundImage.color = priceBackgroundColor;
        if (showIconHolder)
        {
            iconHolder.Show();
        }
        else
        {
            iconHolder.Hide();
        }
    }

    void updateTextColor(int amount)
    {
        if(!game.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt))
        {           
            priceOrAdoptionStatus.color = tuning.UnaffordableTextColor;
        }
        else
        {
            priceOrAdoptionStatus.color = tuning.DefaultPriceColor;
        }
    }

}
