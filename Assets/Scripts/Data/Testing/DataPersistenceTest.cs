/*
 * Author: Isaiah Mann, Grace Barrett-Snyder
 * Description: Testing data serialization
 */

using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataPersistenceTest : MonoBehaviourExtended 
{
	[SerializeField]
	Text coinText;
	[SerializeField]
	Text foodText;
    [SerializeField]
    Text homeSlotsText;

	protected override void fetchReferences() 
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
		dataController.SetFilePath(Path.Combine(Application.persistentDataPath, "TestSave.dat"));
		dataController.SubscribeToCurrencyChange(CurrencyType.Coins, updateCoinsText);
		dataController.SubscribeToCurrencyChange(CurrencyType.DogFood, updateFoodText);
		dataController.SubscribeToCurrencyChange(CurrencyType.HomeSlots, updateHomeSlotsText);
		dataController.LoadGame();

        // Display at start
        updateCoinsText(dataController.Coins.Amount);
        updateFoodText(dataController.DogFood.Amount);
        updateHomeSlotsText(dataController.HomeSlots.Amount);
	}

	public void ChangeCoins(int deltaCoins) 
	{
		dataController.ChangeCoins(deltaCoins);
	}
		
	public void ChangeFood(int deltaFood) 
	{
		dataController.ChangeFood(deltaFood);
	}

    public void ChangeHomeSlots(int deltaHomeSlots)
    {
        dataController.ChangeHomeSlots(deltaHomeSlots);
    }
		
	public void ResetData() 
	{
		dataController.Reset();
	}

	void updateCoinsText(int coins) 
	{
		coinText.text = string.Format("{0}: {1}", "Coins", coins);
	}

	void updateFoodText(int food) 
	{
		foodText.text = string.Format("{0}: {1}", "Food", food);
	}

    void updateHomeSlotsText(int homeSlots)
    {
        HomeSlotsData homeSlotsData = dataController.HomeSlots;
        homeSlotsText.text = "HomeSlots (O/V): " + 
            homeSlotsData.OccupiedSlots + "/" + homeSlotsData.VacantSlots;
    }
		
}
