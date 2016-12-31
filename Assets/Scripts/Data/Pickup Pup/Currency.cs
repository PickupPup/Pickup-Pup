/*
 * Author(s): Grace Barrett-Snyder
 * Description: Defines a type of currency (ex: dog food or coins)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency {

    protected CurrencyType type;
    protected int amount = 0;

    /// <summary>
    /// Constructs new Currency with the specified type and initial amount.
    /// </summary>
    public Currency(CurrencyType type, int initialAmount)
    {
        this.type = type;
        amount = initialAmount;
    }

    /// <summary>
    /// Sets the amount of this currency to the given number.
    /// </summary>
    public void Set(int num)
    {
        amount = num;
    }

    /// <summary>
    /// Increases the amount of this currency by the given number.
    /// </summary>
    public void IncreaseBy(int num)
    {
        amount += num;
    }

    /// <summary>
    /// Decrease the amount of this currency by the given number.
    /// </summary>
    public void DecreaseBy(int num)
    {
        amount -= num;
    }

    /// <summary>
    /// Returns the amount of this currency.
    /// </summary>
    public int Amount
    {
        get { return amount; }
    }
}