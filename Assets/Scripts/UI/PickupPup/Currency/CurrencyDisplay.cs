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

    public void Init(CurrencyData currency, PPDataController dataController)
	{
        this.dataController = dataController;
		unsubscribeEvents();
		type = currency.Type;
        setIcon(currency);
        updateAmount(currency.Amount);
		this.dataController.SubscribeToCurrencyChange(type, updateAmount);
	}

    protected void setIcon(CurrencyData currency)
    {
        iconImage.sprite = currency.Icon;
    }

	protected override void cleanupReferences()
	{
		base.cleanupReferences();
		if(dataController)
		{
			dataController.UnsubscribeFromCurrencyChange(type, updateAmount);
		}
	}

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
	}

	public void updateAmount(int newAmount)
	{
		text.text = newAmount.ToString();
	}

}
