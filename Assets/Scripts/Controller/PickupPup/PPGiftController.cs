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
    WeightedRandomBuffer<CurrencyType> defaultReturnChancesWithGifts;
	WeightedRandomBuffer<CurrencyData> giftChances;

    CurrencyFactory giftFactory;

    #if UNITY_EDITOR

    [Header("Cheats")]
    [SerializeField]
    bool alwaysReturnSouvenirFirst = false;

    #endif

	public void Init(PPTuning tuning)
	{
		this.tuning = tuning;
		defaultReturnChances = new WeightedRandomBuffer<CurrencyType>(
			new CurrencyType[]
			{
				CurrencyType.Coins, 
				CurrencyType.DogFood,
			},
			new float[]
			{
				tuning.DefaultChanceOfCollectingMoney,
				tuning.DefaultChanceOfCollectingDogFood,
			}
		);
        defaultReturnChancesWithGifts = new WeightedRandomBuffer<CurrencyType>(
            new CurrencyType[]
            {
                CurrencyType.Coins, 
                CurrencyType.DogFood,
                CurrencyType.SpecialGift
            },
            new float[]
            {
                tuning.DefaultChanceOfCollectingMoney,
                tuning.DefaultChanceOfCollectingDogFood,
                tuning.ChanceOfSpecialGift
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
    #if UNITY_EDITOR
        
        if(alwaysReturnSouvenirFirst && !dog.SouvenirCollected)
        {
            // TODO: This function should eventually return all types of special gifts, not just souvenirs, will need to be modified here:
            return getSpecialGift(dog);
        }

    #endif
        CurrencyType specialization = dog.Breed.ISpecialization;
        int amount = randomAmount();
        CurrencyType type;
        if(specialization == CurrencyType.None)
        {
            type = defaultRandomType(dog);
        }
        else
        {
            type = getRandomizerBySpecialization(dog, specialization).GetRandom();
        }
        if(type == CurrencyType.SpecialGift)
        {
            return getSpecialGift(dog);
        }
        else
        {
            return giftFactory.Create(type, amount);
        }
    }

    bool eligibleForSouvenir(DogDescriptor dog)
    {
        return !(dog.SouvenirCollected || dog.FirstTimeScouting);
    }

    CurrencyData getSpecialGift(DogDescriptor dog)
    {
        // TODO: Should be able to return other types of special gifts including other dogs, and Tub-Tub the Cat
        return dog.Souvenir;
    }

	public CurrencyData GetDailyGift()
	{
		return giftChances.GetRandom();
	}

	WeightedRandomBuffer<CurrencyType> getRandomizerBySpecialization(DogDescriptor dog, CurrencyType specialization)
	{
        CurrencyType[] currencies;
        float[] weights;
        // TODO: This check needs to be moved inside of special gift logic when we implement other special gifts:
        if(eligibleForSouvenir(dog))
        {
            currencies = new CurrencyType[]
            {
                specialization,
                getSecondary(specialization),
                CurrencyType.SpecialGift,
            };
            weights = new float[]
            {
                tuning.ChanceOfSpecialization,
                tuning.ChanceOfSecondary,
                tuning.ChanceOfSpecialGift,
            };
        }
        else
        {
            currencies = new CurrencyType[]
            {
                specialization,
                getSecondary(specialization),
            };
            weights = new float[]
            {
                tuning.ChanceOfSpecialization,
                tuning.ChanceOfSecondary,
            };
        }
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

    CurrencyType defaultRandomType(DogDescriptor dog)
	{
        // TODO: This check needs to be moved inside of special gift logic when we implement other special gifts:
        if(eligibleForSouvenir(dog))
        {
            return defaultReturnChancesWithGifts.GetRandom();
        }
        else
        {
		    return defaultReturnChances.GetRandom();
        }
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
