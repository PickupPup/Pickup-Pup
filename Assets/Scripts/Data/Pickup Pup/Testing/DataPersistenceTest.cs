/*
 * Author: Isaiah Mann
 * Desc: Testubg data serialization
 */

using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataPersistenceTest : MonoBehaviourExtended 
{

	PPDataController save;

	[SerializeField]
	Text coinText;
	[SerializeField]
	Text foodText;

	protected override void FetchReferences() 
	{
		base.FetchReferences ();
		save = PPDataController.GetInstance;
		save.SetFilePath(Path.Combine(Application.persistentDataPath, "TestSave.dat"));
		save.SubscribeToCoinsChange(updateCoinsText);
		save.SubscribeToFoodChange(updateFoodText);
		save.LoadGame();
	}

	public void ChangeCoins(int deltaCoins) 
	{
		save.ChangeCoins(deltaCoins);
	}
		
	public void ChangeFood(int deltaFood) 
	{
		save.ChangeFood(deltaFood);
	}
		
	public void ResetData() 
	{
		save.Reset();
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
