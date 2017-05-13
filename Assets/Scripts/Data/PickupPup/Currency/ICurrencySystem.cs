/*
 * Author: Grace Barrett-Snyder 
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

    #endregion

    void ChangeCoins(int deltaCoins);
    void ChangeFood(int deltaFood);
	void ChangeCurrencyAmount(CurrencyData currency);
	void SubscribeToCurrencyChange(CurrencyType type, MonoBehaviourExtended.MonoActionInt callback);
	void UnsubscribeFromCurrencyChange(CurrencyType type, MonoBehaviourExtended.MonoActionInt callback);
	void ConvertCurrency(CurrencyData taken, CurrencyData given);
	void GiveCurrency(CurrencyData currency);
	bool TryTakeCurrency(CurrencyData currency);
    bool CanAfford(CurrencyType type, int amount);
    bool HasCurrency(CurrencyType type);
	
    // Added new because this method exists in the parent interface (extends polymorphism)
    new bool TryUnsubscribeAll();

}
