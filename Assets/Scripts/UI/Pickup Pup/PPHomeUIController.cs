/*
 * 
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the home screen
 */

using UnityEngine;

public class PPHomeUIController : PPUIController 
{
	[SerializeField]
    CurrencyDisplay dogFoodDisplay;
	[SerializeField]
    CurrencyDisplay coinDisplay;

	#region MonoBehaviourExtended Overrides

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		EventController.Event(PPEvent.LoadHome);
        // Set Currency Displays
        dogFoodDisplay.SetCurrency(gameController.DogFood);
        coinDisplay.SetCurrency(gameController.Coins);
    }

	#endregion

}
