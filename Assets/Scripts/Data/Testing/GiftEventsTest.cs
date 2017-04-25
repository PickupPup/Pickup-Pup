/*
 * Author(s): Isaiah Mann
 * Description: Unit test for verifying gift event
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
		Debug.Log(testEvent);
        for(int i = 0; i < numberTimesTestSpawnCurrency; i++)
        {
			Debug.Log(testEvent.GetResult().Length);
            Debug.LogFormat("Coins: {0}, Food: {1}", dataController.Coins, dataController.DogFood);
			testEvent.Give();
            Debug.LogFormat("Coins: {0}, Food: {1}", dataController.Coins, dataController.DogFood);
        }
	}

}
