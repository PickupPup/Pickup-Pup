/*
 * Author(s): Isaiah Mann
 * Description: Displays a dog's souvenir
 * Usage: [no notes]
 */

using UnityEngine;

public class SouvenirDisplay : CurrencyDisplay
{
    [SerializeField]
    PPUIElement nameDisplay;
    [SerializeField]
    PPUIElement descriptionDisplay;

    [SerializeField]
    PPUIButton displayButton;
    [SerializeField]
    PPUIElement hideButton;

}
