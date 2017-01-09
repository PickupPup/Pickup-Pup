/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls UI of Currency
 */

public class CurrencyDisplay : PPUIElement 
{
    CurrencyData currency;

    // Sets the Currency to display
    public void SetCurrency(CurrencyData currency)
    {
        this.currency = currency;
        OnUpdate();
    }

    // Updates text to show the new amount of Currency
    public void OnUpdate()
    {
        text.text = currency.Amount.ToString();
    }    

}
