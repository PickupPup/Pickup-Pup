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

    CurrencyFactory giftFactory;

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
        giftFactory = new CurrencyFactory();
	}

	public CurrencyData GetGiftFromDog(DogDescriptor dog)
	{
        CurrencyData gift;
        if(tryGetExistingGift(dog, out gift))
        {
            return gift;
        }
        else
        {
            return generateGift(dog);
        }
	}

    bool tryGetExistingGift(DogDescriptor info, out CurrencyData gift)
    {
        if(info.IsLinkedToDog)
        {
            Dog dog = info.PeekDogLink;
            if(dog.HasRedeemableGift)
            {
                gift = dog.PeekAtGift;
                return true;
            }
            else 
            {
                gift = null;
                return false;
            }
        }
        else
        {
            gift = null;
            return false;
        }
    }

    CurrencyData generateGift(DogDescriptor dog)
    {
        CurrencyType specialization = dog.Breed.ISpecialization;
        int amount = randomAmount();
        CurrencyType type;
        if(specialization == CurrencyType.None)
        {
            type = defaultRandomType();
        }
        else
        {
            type = getRandomizerBySpecialization(specialization).GetRandom();
        }
        return giftFactory.Create(type, amount);
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
			CurrencyType.SpecialGift,
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
