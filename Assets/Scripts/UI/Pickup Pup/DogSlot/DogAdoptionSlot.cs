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

    Color adoptedColor = Color.red;
    Color unaffordableColor = Color.red;
    Color adoptedTextColor = Color.white;

    PPDataController dataController;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if(dataController)
        {
            dataController.SubscribeToCoinsChange(updateTextColor);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if(dataController)
        {
            dataController.UnsubscribeFromCoinsChange(updateTextColor);
        }
    }

    #endregion

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite)
    {
        unsubscribeEvents();
        base.Init(dog, dogSprite);
        subscribeEvents();

        priceOrAdoptionStatus = GetComponentInChildren<Text>();
        priceOrAdoptionStatus.text = dog.CostToAdoptStr;
        iconHolder.Show();

        checkReferences();
        updateTextColor(game.Coins.Amount);
    }

    #endregion

    public void ShowAdopt()
    {
        priceOrAdoptionStatus.text = "ADOPTED";
        priceOrAdoptionStatus.color = adoptedTextColor;
        priceBackgroundImage.color = adoptedColor;
        iconHolder.Hide();
        unsubscribeEvents();
    }

    void updateTextColor(int amount)
    {
        if(!game.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt))
        {           
            priceOrAdoptionStatus.color = unaffordableColor;
        }
    }

}
