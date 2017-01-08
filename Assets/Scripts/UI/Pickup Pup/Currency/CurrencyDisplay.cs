/*
 * Author(s): Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls UI of Currency
 */

public class CurrencyDisplay : PPUIElement 
{
    Currency currency;

    /// <summary>
    /// Sets the Currency to display
    /// </summary>
    public void SetCurrency(Currency currency)
    {
        this.currency = currency;
        OnUpdate();
    }

    /// <summary>
    /// Updates text to show the new amount of Currency
    /// </summary>
    public void OnUpdate()
    {
        text.text = currency.Amount.ToString();
    }    

}
