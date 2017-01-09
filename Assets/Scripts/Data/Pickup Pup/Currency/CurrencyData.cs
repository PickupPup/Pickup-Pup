/*
 * Author: Grace Barrett-Snyder, Isaiah Mann
 * Description: Holds the data for a specific form of currency (ex: coins)
 */

[System.Serializable]
public class CurrencyData : PPData
{
    protected CurrencyType type;
    protected int amount = 0;

    public CurrencyData(CurrencyType type, int initialAmount)
    {
        this.type = type;
        amount = initialAmount;
    }

    protected CurrencyData(int initialAmount)
    {
        // NOTHING
    }

    public void IncreaseBy(int deltaAmount)
    {
        amount += deltaAmount;
    }

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public CurrencyType Type
    {
        get
        {
            return type;
        }
    }

}
