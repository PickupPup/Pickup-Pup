/*
 * Author(s): Isaiah Mann
 * Description: Produces currencies from numerical / JSON args
 * Usage: [no notes]
 */

using System;
using System.Reflection;

using k = PPGlobal;

[System.Serializable]
public class CurrencyFactory : ObjectFactory<CurrencyData>
{
	const string CLASS_NAME_FORMAT = "{0}Data";

	GiftDatabase gifts
	{
		get
		{
			return GiftDatabase.GetInstance;
		}
	}

    // Expects: (string type, int amount, float percent (if discout))
    public override CurrencyData Create (params object[] args)
    {
		string typeStr = args[0].ToString();
		CurrencyType type = (CurrencyType) Enum.Parse(typeof(CurrencyType), typeStr);
		bool hasAmount;
		int amount;
		try
		{
			amount = (int) args[1];
			hasAmount = true;
		}
		catch
		{
			amount = k.INVALID_VALUE;
			hasAmount = false;
		}
		if(type == CurrencyType.DogVoucher)
		{
			return new DogVoucherData();
		}
		else
        {
			Type currencyClassType = Type.GetType(getClassName(typeStr));
			ConstructorInfo currencyConstructor = currencyClassType.GetConstructor(new Type[]
				{
					typeof(int)
				});
			if(hasAmount)
			{

				return currencyConstructor.Invoke(new object[]
					{
						amount
					}) as CurrencyData;
			}
			else if(type == CurrencyType.GiftEvent)
			{
				try
				{
					return this.gifts.GetRandomGiftEvent();
				}
				catch(GiftEventNotAvailableException exception)
				{
					throw exception;
				}
			}
			else
			{
				return currencyConstructor.Invoke(new object[]
					{
						k.SINGLE_VALUE
					}) as CurrencyData;
			}
        }
    }
		
	string getClassName(string type)
	{
		return string.Format(CLASS_NAME_FORMAT, type);
	}
		
	// Expects: (ParallelArray<string, int>, float percent (if the list includes discounts))
    public override CurrencyData[] CreateGroup(params object[] args)
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

	public CurrencyData[] CreateGroup(string[] currencyTypes)
	{
		CurrencyData[] currencies = new CurrencyData[currencyTypes.Length];
		for(int i = 0; i < currencies.Length; i++)
		{
			currencies[i] = Create(currencyTypes[i]);
		}
		return currencies;
	}

}
