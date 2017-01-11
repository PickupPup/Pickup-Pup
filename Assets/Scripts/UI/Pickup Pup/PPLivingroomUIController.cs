/*
 * Author: James Hostetler
 * Description: Controls the Livingroom UI
 */

using UnityEngine;
using UnityEngine.UI;

public class PPLivingroomUIController : PPUIController
{
	[SerializeField]
    CurrencyDisplay dogFoodDisplay;
	[SerializeField]
    CurrencyDisplay coinDisplay;

	#region MonoBehaviourExtended Overrides

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		EventController.Event(PPEvent.LoadLivingroom);
		// Set Currency Displays
        dogFoodDisplay.SetCurrency(gameController.DogFood);
        coinDisplay.SetCurrency(gameController.Coins);
    }

	#endregion
}
