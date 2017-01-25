/*
 * Author(s): Isaiah Mann
 * Description: Determines what kind of gift the player receives from dogs scouting
 * Usage: [no notes]
 */

using UnityEngine;
using k = PPGlobal;

public class PPGiftController : SingletonController<PPGiftController>
{	
	const float DEFAULT_DISCOUNT = k.DEFAULT_DISCOUNT_DECIMAL;

	PPTuning tuning;
	WeightedRandomBuffer<CurrencyType> defaultReturnChances;
	WeightedRandomBuffer<CurrencyData> giftChances;

	public void Init(PPTuning tuning)
	{
		this.tuning = tuning;
		defaultReturnChances = new WeightedRandomBuffer<CurrencyType>(
			new CurrencyType[]
			{
				CurrencyType.Coins, 
				CurrencyType.DogFood
			},
			new float[]
			{
				tuning.DefaultChanceOfCollectingMoney,
				tuning.DefaultChanceOfCollectingDogFood
			}
		);
		giftChances = populateGifts(
			tuning.DailyGiftOptions,
			tuning.DailyGiftAmounts,
			tuning.DailyGiftWeights,
			tuning.DailyGiftDiscountAmount);
	}

	public CurrencyData GetGiftFromDog(DogDescriptor dog)
	{
		CurrencyType specialization = dog.Breed.ISpecialization;
		if(specialization == CurrencyType.None)
		{
			return new CurrencyData(defaultRandomType(), randomAmount());
		}
		else
		{
			CurrencyType type = getRandomizerBySpecialization(specialization).GetRandom();
			return new CurrencyData(type, randomAmount());
		}
	}

	public CurrencyData GetDailyGift()
	{
		return giftChances.GetRandom();
	}

	WeightedRandomBuffer<CurrencyType> getRandomizerBySpecialization(CurrencyType specialization)
	{
		CurrencyType[] currencies = new CurrencyType[]
		{
			specialization,
			getSecondary(specialization),
			CurrencyType.SpecialObject,
		};
		float[] weights = new float[]
		{
			tuning.ChanceOfSpecialization,
			tuning.ChanceOfSecondary,
            tuning.ChanceOfSpecialGift,
		};
		return new WeightedRandomBuffer<CurrencyType>(currencies, weights);
	}

	WeightedRandomBuffer<CurrencyData> populateGifts(
		string[] giftTypes, 
		int[] giftAmounts, 
		float[] giftChances,
		float discountPercent)
	{
		ParallelArray<string, int> giftData = new ParallelArray<string, int>(giftTypes, giftAmounts);
		CurrencyFactory giftFactory = new CurrencyFactory();
		CurrencyData[] gifts = giftFactory.CreateGroup(giftData, discountPercent);
		return new WeightedRandomBuffer<CurrencyData>(gifts, giftChances);
	}

	CurrencyType defaultRandomType()
	{
		return defaultReturnChances.GetRandom();
	}

	int randomAmount()
	{
		int zeroOffset = 1;
		return Random.Range(zeroOffset, tuning.MaxAmountPerTypeFromScouting + zeroOffset);
	}

	CurrencyType getSecondary(CurrencyType specialization)
	{
		switch(specialization)
		{
			case CurrencyType.Coins:
				return CurrencyType.DogFood;
			case CurrencyType.DogFood:
				return CurrencyType.Coins;
			default:
				return CurrencyType.None;
		}
	}

}
