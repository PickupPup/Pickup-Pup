/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;

public class GiftEventsTest : MonoBehaviourExtended 
{	
    GiftDatabase gifts;

    [SerializeField]
    int  numberTimesTestSpawnCurrency = 5;

	protected override void fetchReferences() 
	{
        base.fetchReferences();
        gifts = GiftDatabase.GetInstance;
        GiftEventData testEvent = gifts.GiftEvents[0];
        Debug.Log(testEvent.EventDescription);
        for(int i = 0; i < numberTimesTestSpawnCurrency; i++)
        {
            Debug.Log(testEvent.GetCurrencies().Length);
            Debug.LogFormat("Coins: {0}, Food: {1}", dataController.Coins, dataController.DogFood);
            testEvent.Call();
            Debug.LogFormat("Coins: {0}, Food: {1}", dataController.Coins, dataController.DogFood);
        }
	}

}
