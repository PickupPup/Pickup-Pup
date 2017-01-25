/*
 * Author(s): Isaiah Mann
 * Description: Produces currencies from numerical / JSON args
 * Usage: [no notes]
 */

using System;
using System.Reflection;

[System.Serializable]
public class CurrencyFactory : ObjectFactory<CurrencyData>
{
	const string CLASS_NAME_FORMAT = "{0}Data";

    // Expects: (string type, int amount, float percent (if discout))
    public override CurrencyData Create (params object[] args)
    {
		string typeStr = args[0].ToString();
		CurrencyType type = (CurrencyType) Enum.Parse(typeof(CurrencyType), typeStr);
        int amount = (int) args[1];
		if(type == CurrencyType.DogDiscount)
        {
			if(args.Length >= 3)
			{
				return new DiscountData((float) args[2], amount);
			}
			else 
			{
				return new DiscountData(DEFAULT_DISCOUNT, amount);
			}
        }
        else
        {
			Type currencyClassType = Type.GetType(getClassName(typeStr));
			ConstructorInfo currencyConstructor = currencyClassType.GetConstructor(new Type[]
				{
					typeof(int)
				});
			return currencyConstructor.Invoke(new object[]
				{
					amount
				}) as CurrencyData;
        }
    }

	string getClassName(string type)
	{
		return string.Format(CLASS_NAME_FORMAT, type);
	}
		
	// Expects: (ParallelArray<string, int>, float percent (if the list includes discounts))
    public override CurrencyData[] CreateGroup (params object[] args)
    {
		ParallelArray<string, int> currencyData = args[0] as ParallelArray<string, int>;
		float discountPercent;
		if(args.Length > 1)
		{
			discountPercent = (float) args[1];
		}
		else
		{
			discountPercent = DEFAULT_DISCOUNT;
		}
		CurrencyData[] currencies = new CurrencyData[currencyData.Length];
		for(int i = 0; i < currencies.Length; i++)
		{
			currencies[i] = Create(currencyData[i].First, currencyData[i].Second, discountPercent);
		}
		return currencies;
    }

}
