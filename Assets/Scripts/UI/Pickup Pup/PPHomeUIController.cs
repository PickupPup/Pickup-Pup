/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls the home screen
 */

public class PPHomeUIController : PPUIController {

    public CurrencyDisplay dogFoodDisplay;
    public CurrencyDisplay coinDisplay;

    protected override void FetchReferences () {
		base.FetchReferences ();
		EventController.Event(PPEvent.LoadHome);

        // Set Currency Displays
        dogFoodDisplay.SetCurrency(ppGameController.DogFood);
        coinDisplay.SetCurrency(ppGameController.Coins);
    }
}