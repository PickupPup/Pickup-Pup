/*
 * Author: Isaiah Mann
 * Description: Testing data serialization
 */

using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataPersistenceTest : MonoBehaviourExtended 
{
	PPDataController dataController;

	[SerializeField]
	Text coinText;
	[SerializeField]
	Text foodText;

	protected override void fetchReferences() 
	{
		base.fetchReferences ();
		dataController = PPDataController.GetInstance;
		dataController.SetFilePath(Path.Combine(Application.persistentDataPath, "TestSave.dat"));
		dataController.SubscribeToCoinsChange(updateCoinsText);
		dataController.SubscribeToFoodChange(updateFoodText);
		dataController.LoadGame();
	}

	public void ChangeCoins(int deltaCoins) 
	{
		dataController.ChangeCoins(deltaCoins);
	}
		
	public void ChangeFood(int deltaFood) 
	{
		dataController.ChangeFood(deltaFood);
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
		
}
