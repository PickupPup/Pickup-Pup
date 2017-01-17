/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls UI of Currency
 */

using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : PPUIElement 
{
    [SerializeField]
    Image iconImage;

	CurrencyType type;
	PPDataController dataController;

    public void Init(PPDataController dataController, CurrencyData currency)
	{
		unsubscribeEvents();
		this.dataController = dataController;

		type = currency.Type;
        iconImage.sprite = currency.Icon;
		subscribeEvents();
        updateAmount(currency.Amount);
	}

	public void updateAmount(int newAmount)
	{
		text.text = newAmount.ToString();
	}

}
