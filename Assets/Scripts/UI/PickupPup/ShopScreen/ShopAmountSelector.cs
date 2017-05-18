/*
 * Author(s): Isaiah Mann
 * Description: UI popup for the player to select an amount they want to purchase in the shop
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class ShopAmountSelector : PPUIElement
{
	[SerializeField]
	Image iconDisplay;
	[SerializeField]
	Text descriptionDisplay;
	[SerializeField]
	Text amountDisplay;
	[SerializeField]
	Text costDisplay;
	[SerializeField]
	PPUIButton increaseAmountButton;
	[SerializeField]
	PPUIButton decreaseAmountButton;
	[SerializeField]
	PPUIButton buyButton;
	[SerializeField]
	PPUIButton closeButton;

	ShopItem item;
	int amountToPurchase = k.SINGLE_VALUE;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		increaseAmountButton.SubscribeToClick(increasePurchaseAmount);
		decreaseAmountButton.SubscribeToClick(decreasePurchaseAmount);
		buyButton.SubscribeToClick(purchaseItem);
		closeButton.SubscribeToClick(Hide);
	}

	#endregion

	#region UIElement Overrides

	public override void Hide()
	{
		base.Hide();
		amountToPurchase = k.SINGLE_VALUE;
	}

	#endregion

	public void SetItem(ShopItem item)
	{
		checkReferences();
		this.item = item;
		setupDisplay();
	}

	void increasePurchaseAmount()
	{
		changePurchaseAmount(k.SINGLE_VALUE);
	}

	void decreasePurchaseAmount()
	{
		changePurchaseAmount(-k.SINGLE_VALUE);
	}

	void changePurchaseAmount(int deltaAmount)
	{
		amountToPurchase += deltaAmount;
		amountToPurchase = Mathf.Clamp(amountToPurchase, k.SINGLE_VALUE, int.MaxValue);
		updateDisplay();
	}

	void setupDisplay()
	{
		descriptionDisplay.text = item.ItemName;
		iconDisplay.sprite = item.Icon;
		iconDisplay.color = item.IconColor;
		updateDisplay();
	}

	void updateDisplay()
	{
		amountDisplay.text = amountToPurchase.ToString();
		costDisplay.text = item.GetTotalCostStr(amountToPurchase);
		bool canAffordOneMore = gameController.CanAfford(item.CostCurrencyType, item.GetTotalCost(amountToPurchase + k.SINGLE_VALUE));
		increaseAmountButton.ToggleEnabled(canAffordOneMore);
		decreaseAmountButton.ToggleEnabled(amountToPurchase > k.SINGLE_VALUE);
	}

	void purchaseItem()
	{
		if(gameController.TryBuyItem(item, amountToPurchase))
		{
			analytics.SendEvent(
				new CurrencyAnalyticsEvent(
					CurrencyAnalyticsEvent.SHOP_PURCHASE,
					new CurrencyFactory().Create(
						item.ValueCurrencyType,
						amountToPurchase)));
		}
		Hide();
	}

}
