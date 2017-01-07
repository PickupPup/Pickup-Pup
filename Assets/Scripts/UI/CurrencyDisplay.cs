/*
 * Author(s): Grace Barrett-Snyder
 * Description: Controls UI of Currency
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDisplay : UIElement 
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
