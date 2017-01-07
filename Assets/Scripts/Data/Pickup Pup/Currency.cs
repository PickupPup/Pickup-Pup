/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Defines a type of currency (ex: dog food or coins)
 */

[System.Serializable]
public class Currency 
{
	#region Static Accessors

	public static Currency[] Defaults 
	{
		get
		{
			return new Currency[]
			{
				new Currency(CurrencyType.Coins, 2000),
				new Currency(CurrencyType.DogFood, 0),
                new Currency(CurrencyType.VacantHomeSlots, 10)
			};
		}
	}

	#endregion

    protected CurrencyType type;
    protected int amount = 0;

    public Currency(CurrencyType type, int initialAmount)
    {
        this.type = type;
        amount = initialAmount;
    }

    public void Set(int num)
    {
        amount = num;
    }

    public void IncreaseBy(int num)
    {
        amount += num;
    }

    public void DecreaseBy(int num)
    {
        amount -= num;
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
