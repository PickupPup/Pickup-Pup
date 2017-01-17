/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the display of multiple currencies on the Currency Panel
 */

using UnityEngine;

public class CurrencyPanel : SingletonController<CurrencyPanel>
{
    [SerializeField]
    CurrencyDisplay coinsDisplay;
    [SerializeField]
    CurrencyDisplay dogFoodDisplay;

    PPDataController dataController;

    public void Init(PPDataController dataController)
    {
        unsubscribeEvents();
        this.dataController = dataController;
        subscribeEvents();

        // Display Updated Currency
        dogFoodDisplay.Init(dataController, dataController.DogFood);
        coinsDisplay.Init(dataController, dataController.Coins);    
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if(dataController)
        {
            dataController.SubscribeToCoinsChange(updateCoinsDisplay);
            dataController.SubscribeToFoodChange(updateDogFoodDisplay);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if(dataController)
        {
            dataController.UnsubscribeFromCoinsChange(updateCoinsDisplay);
            dataController.UnsubscribeFromFoodChange(updateDogFoodDisplay);
        }
    }

    void updateCoinsDisplay(int newAmount)
    {
        coinsDisplay.updateAmount(newAmount);
    }

    void updateDogFoodDisplay(int newAmount)
    {
        dogFoodDisplay.updateAmount(newAmount);
    }

}
