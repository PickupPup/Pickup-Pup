/*
 * Author: Isaiah Mann
 * Desc: Used to store tuning variables
 */

[System.Serializable]
public class PPTuning : PPData 
{
	
	public int MaxDogsScouting;
	public int MaxDogsInHouse;
	public int DailyCoinBonus;
	public int DailyNumberOfNewDogsAtShelter;
	public int MinDogsToUnlockCollar;
	public int MinDogFoodToUnlockCollar;
	public float TimeScoutingToTimeRestingRatio;
	public float ChanceOfDogsEatingPoop;
	public float MinCouponDiscount;
	public float MaxCouponDiscount;
	public int MissedFeedingsBeforeSeizure;
	public float ShelterResellCostScale;
	public float DefaultChanceOfCollectingDogFood;
	public float DefaultChanceOfCollectingMoney;
	public int MaxPuppyAge;
	public int MinSeniorDogAge;
	public float PuppyChanceOfRunningAwayDuringScounting;
	public float PuppyScountingTimeModifier;
	public float SeniorDogScoutingTimeModifier;
	public int MaxAmountPerTypeFromScouting;
	public float ChanceOfRareObject;
	public float ChanceSpecialItemIsPresent;
	public float ChangeSpecialItemIsDog;
	public float ChangeSpecialItemIsTubTub;
	public int SpecialDogCount;
	public int MaxNumberAdsPerDay;
	public int VideoAdCoinBonus;
	public int CostOfOneDogFoodCan;
	public int CostOfFiveDogFoodCans;
	public int CostOfTenDogFoodCans;
	public int CostOfTwentyDogFoodCans;

}
