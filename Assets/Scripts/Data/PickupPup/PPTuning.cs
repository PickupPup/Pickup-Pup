/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Used to store tuning variables
 */

using UnityEngine;

[System.Serializable]
public class PPTuning : PPData 
{
	#region Instance Acessors

    public string AdoptedText
    {
        get
        {
            return adoptedText;
        }
    }

    public string AdoptText
    {
        get
        {
            return adoptText;
        }
    }

    public float AffectionIncrease
    {
        get
        {
            return affectionIncrease;
        }
    }

    public float ChanceForNoSpecial
    {
        get
        {
            return chanceForNoSpecial;
        }
    }

    public float ChanceOfDogsEatingPoop
    {
        get
        {
            return chanceOfDogsEatingPoop;
        }
    }

    public float ChanceOfRareObject
    {
        get
        {
            return chanceOfRareObject;
        }
    }

    public float ChanceOfSecondary
    {
        get
        {
            return chanceOfSecondary;
        }
    }

    public float ChanceOfSpecialGift
    {
        get
        {
            return chanceOfSpecialGift;
        }
    }

    public float ChanceOfSpecialization
    {
        get
        {
            return chanceOfSpecialization;
        }
    }

    public float ChanceSpecialItemIsDog
    {
        get
        {
            return chanceSpecialItemIsDog;
        }
    }

    public float ChanceSpecialItemIsPresent
    {
        get
        {
            return chanceSpecialItemIsPresent;
        }
    }

    public float ChanceSpecialItemIsTubTub
    {
        get
        {
            return chanceSpecialItemIsTubTub;
        }
    }

    public int CostOfFiveDogFoodCans
    {
        get
        {
            return costOfFiveDogFoodCans;
        }
    }

    public int CostOfOneDogFoodCan
    {
        get
        {
            return costOfOneDogFoodCan;
        }
    }

    public int CostOfTenDogFoodCans
    {
        get
        {
            return costOfTenDogFoodCans;
        }
    }

    public int CostOfTwentyDogFoodCans
    {
        get
        {
            return costOfTwentyDogFoodCans;
        }
    }

    public int[] DailyGiftAmounts
    {
        get
        {
            return dailyGiftAmounts;
        }
    }

    public float DailyGiftDiscountAmount
    {
        get
        {
            return dailyGiftDiscountAmount;
        }
    }

    public string[] DailyGiftOptions
    {
        get
        {
            return dailyGiftOptions;
        }
    }

    public float[] DailyGiftWeights
    {
        get
        {
            return dailyGiftWeights;
        }
    }

    public int DailyNumberOfNewDogsAtShelter
    {
        get
        {
            return dailyNumberOfNewDogsAtShelter;
        }
    }

    public Color DefaultBackgroundColor
    {
        get
        {
            return getColor(defaultBackgroundColorRGBA);
        }
    }

    public float DefaultChanceOfCollectingDogFood
    {
        get
        {
            return defaultChanceOfCollectingDogFood;
        }
    }

    public float DefaultChanceOfCollectingMoney
    {
        get
        {
            return defaultChanceOfCollectingMoney;
        }
    }

    public Color DefaultPriceColor
    {
        get
        {
            return getColor(defaultPriceColorRGBA);
        }
    }

    public Color DefaultTextColor
    {
        get
        {
            return getColor(defaultTextColorRGBA);
        }
    }

    public float DefaultTimerTimeStepSec
    {
        get
        {
            return defaultTimerTimeStepSec;
        }
    }

    public float DogFoodFeedTimeSec
    {
        get
        {
            return dogFoodFeedTimeSec;
        }
    }

    public string InWorldKey
    {
        get
        {
            return inWorldKey;
        }
    }

    public float MaxAffection
    {
        get
        {
            return maxAffection;
        }
    }

    public int MaxAmountPerTypeFromScouting
    {
        get
        {
            return maxAmountPerTypeFromScouting;
        }
    }

    public float MaxCouponDiscount
    {
        get
        {
            return maxCouponDiscount;
        }
    }

    public int MaxDogsInHouse
    {
        get
        {
            return maxDogsInHouse;
        }
    }

    public int MaxDogsScouting
    {
        get
        {
            return maxDogsScouting;
        }
    }

    public int MaxNumberAdsPerDay
    {
        get
        {
            return maxNumberAdsPerDay;
        }
    }

    public int MaxPuppyAge
    {
        get
        {
            return maxPuppyAge;
        }
    }

    public float MinCouponDiscount
    {
        get
        {
            return minCouponDiscount;
        }
    }

    public int MinDogFoodToUnlockCollar
    {
        get
        {
            return minDogFoodToUnlockCollar;
        }
    }

    public int MinDogsToUnlockCollar
    {
        get
        {
            return minDogsToUnlockCollar;
        }
    }

    public int MinSeniorDogAge
    {
        get
        {
            return minSeniorDogAge;
        }
    }

    public int MissedFeedingsBeforeSeizure
    {
        get
        {
            return missedFeedingsBeforeSeizure;
        }
    }

    public Color NonPurchasableBackgroundColor
    {
        get
        {
            return getColor(nonPurchasableBackgroundColorRGBA);
        }
    }

    public Color NonPurchasableTextColor
    {
        get
        {
            return getColor(nonPurchasableTextColorRGBA);
        }
    }

    public float PuppyChanceOfRunningAwayDuringScouting
    {
        get
        {
            return puppyChanceOfRunningAwayDuringScouting;
        }
    }

    public float PuppyScoutingTimeModifier
    {
        get
        {
            return puppyScoutingTimeModifier;
        }
    }

    public bool SampleShelterDogsInOrder
    {
        get
        {
            return sampleShelterDogsInOrder;
        }
    }

    public float SeniorDogScoutingTimeModifier
    {
        get
        {
            return seniorDogScoutingTimeModifier;
        }
    }

    public int ShelterDogsLimit
    {
        get
        {
            return shelterDogsLimit;
        }
    }

    public float ShelterResellCostScale
    {
        get
        {
            return shelterResellCostScale;
        }
    }

    public bool ShouldLimitShelterDogs
    {
        get
        {
            return shouldLimitShelterDogs;
        }
    }

    public int SpecialDogCount
    {
        get
        {
            return specialDogCount;
        }
    }

    public int StartingCoins
    {
        get
        {
            return startingCoins;
        }
    }

    public int StartingDogCount
    {
        get
        {
            return startingDogCount;
        }
    }

    public int StartingDogFood
    {
        get
        {
            return startingDogFood;
        }
    }

    public int StartingHomeSlots
    {
        get
        {
            return startingHomeSlots;
        }
    }

    public float TimeScoutingToTimeRestingRatio
    {
        get
        {
            return timeScoutingToTimeRestingRatio;
        }
    }

    public Color UnaffordableTextColor
    {
        get
        {
            return getColor(unaffordableTextColorRGBA);
        }
    }

    public int VideoAdCoinBonus
    {
        get
        {
            return videoAdCoinBonus;
        }
    }

    public float WaitTimeSecsForDailyGift
    {
        get
        {
            return waitTimeSecsForDailyGift;
        }
    }
        
    #endregion

    #region JSON Fields

    [SerializeField]
    string adoptedText;
    [SerializeField]
    string adoptText;
    [SerializeField]
    float affectionIncrease;
    [SerializeField]
    float chanceForNoSpecial;
    [SerializeField]
    float chanceOfDogsEatingPoop;
    [SerializeField]
    float chanceOfRareObject;
    [SerializeField]
    float chanceOfSecondary;
    [SerializeField]
    float chanceOfSpecialGift;
    [SerializeField]
    float chanceOfSpecialization;
    [SerializeField]
    float chanceSpecialItemIsDog;
    [SerializeField]
    float chanceSpecialItemIsPresent;
    [SerializeField]
    float chanceSpecialItemIsTubTub;
    [SerializeField]
    int costOfFiveDogFoodCans;
    [SerializeField]
    int costOfOneDogFoodCan;
    [SerializeField]
    int costOfTenDogFoodCans;
    [SerializeField]
    int costOfTwentyDogFoodCans;
    [SerializeField]
    int[] dailyGiftAmounts;
    [SerializeField]
    float dailyGiftDiscountAmount;
    [SerializeField]
    string[] dailyGiftOptions;
    [SerializeField]
    float[] dailyGiftWeights;
    [SerializeField]
    int dailyNumberOfNewDogsAtShelter;
    [SerializeField]
    int[] defaultBackgroundColorRGBA;
    [SerializeField]
    float defaultChanceOfCollectingDogFood;
    [SerializeField]
    float defaultChanceOfCollectingMoney;
    [SerializeField]
    int[] defaultPriceColorRGBA;
    [SerializeField]
    int[] defaultTextColorRGBA;
    [SerializeField]
    float defaultTimerTimeStepSec;
    [SerializeField]
    float dogFoodFeedTimeSec;
    [SerializeField]
    string inWorldKey;
    [SerializeField]
    float maxAffection;
    [SerializeField]
    int maxAmountPerTypeFromScouting;
    [SerializeField]
    float maxCouponDiscount;
    [SerializeField]
    int maxDogsInHouse;
    [SerializeField]
    int maxDogsScouting;
    [SerializeField]
    int maxNumberAdsPerDay;
    [SerializeField]
    int maxPuppyAge;
    [SerializeField]
    float minCouponDiscount;
    [SerializeField]
    int minDogFoodToUnlockCollar;
    [SerializeField]
    int minDogsToUnlockCollar;
    [SerializeField]
    int minSeniorDogAge;
    [SerializeField]
    int missedFeedingsBeforeSeizure;
    [SerializeField]
    int[] nonPurchasableBackgroundColorRGBA;
    [SerializeField]
    int[] nonPurchasableTextColorRGBA;
    [SerializeField]
    float puppyChanceOfRunningAwayDuringScouting;
    [SerializeField]
    float puppyScoutingTimeModifier;
    [SerializeField]
    bool sampleShelterDogsInOrder;
    [SerializeField]
    float seniorDogScoutingTimeModifier;
    [SerializeField]
    int shelterDogsLimit;
    [SerializeField]
    float shelterResellCostScale;
    [SerializeField]
    bool shouldLimitShelterDogs;
    [SerializeField]
    int specialDogCount;
    [SerializeField]
    int startingCoins;
    [SerializeField]
    int startingDogCount;
    [SerializeField]
    int startingDogFood;
    [SerializeField]
    int startingHomeSlots;
    [SerializeField]
    float timeScoutingToTimeRestingRatio;
    [SerializeField]
    int[] unaffordableTextColorRGBA;
    [SerializeField]
    int videoAdCoinBonus;
    [SerializeField]
    float waitTimeSecsForDailyGift;

    #endregion

    Color getColor(int[] rgba)
    {
        return getColor(convertColorValues(rgba));
    }

    Color getColor(float[] rgba)
    {
        return new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
    }

    float[] convertColorValues(int[] rgba)
    {
        float[] convertedValues = new float[rgba.Length];
        for(int i = 0; i < rgba.Length; i++)
        {
            convertedValues[i] = rgba[i] / 255f;
        }
        return convertedValues;
    }

}
