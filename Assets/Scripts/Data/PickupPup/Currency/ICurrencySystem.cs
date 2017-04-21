/*
 * Author: Grace Barrett-Snyder, Ben Page
 * Description: An interface to facilitate data flow of currency changes
 */

public interface ICurrencySystem : ISubscribable
{
    #region Instance Accessors

    CoinsData Coins
    {
        get;
    }

    DogFoodData DogFood
    {
        get;
    }

    HomeSlotsData HomeSlots
    {
        get;
    }

    #endregion

    void ChangeCoins(int deltaCoins);
    void ChangeFood(int deltaFood, DogFoodType dogFoodType);
    void ChangeHomeSlots(int deltaHomeSlots);
    void ChangeCurrencyAmount(CurrencyType type, int deltaAmount);
	void SubscribeToCurrencyChange(CurrencyType type, MonoBehaviourExtended.MonoActionInt callback);
	void UnsubscribeFromCurrencyChange(CurrencyType type, MonoBehaviourExtended.MonoActionInt callback);
    void ConvertCurrency(int value, CurrencyType valueCurrencyType,
        int cost, CurrencyType costCurrencyType);
	void GiveCurrency(CurrencyData currency);
	bool TryTakeCurrency(CurrencyData currency);
    bool CanAfford(CurrencyType type, int amount);
    bool HasCurrency(CurrencyType type);
	
    // Added new because this method exists in the parent interface (extends polymorphism)
    new bool TryUnsubscribeAll();

}
