/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class ShopItemSlot : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image itemImage;

    [SerializeField]
    PriceTag priceTag;
    Button button;

    ShopItem item;

	int amountToBuy = k.SINGLE_VALUE;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        button = GetComponent<Button>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if (dataController)
        {
            dataController.SubscribeToCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if (dataController)
        {
            dataController.UnsubscribeFromCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    #endregion

    public void Init(ShopItem item)
    {
        subscribeEvents();
        this.item = item;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
        priceTag.Set(item.Cost);
        priceTag.ShowPurchasable();
        itemImage.sprite = item.Icon;
		itemImage.color = item.IconColor;
    }

    public void Buy()
    {
		if(gameController.TryBuyItem(item, amountToBuy))
        {
            analytics.SendEvent(
                new CurrencyAnalyticsEvent(
                    CurrencyAnalyticsEvent.SHOP_PURCHASE,
                    new CurrencyFactory().Create(
                        item.ValueCurrencyType,
						amountToBuy)));
        }
    }

	void resetAfterPurchase()
	{
		amountToBuy = k.SINGLE_VALUE;
	}

    void tryToggle(int amount)
    {
        if(button)
        {
			button.interactable = gameController.CanAfford(
				item.CostCurrencyType, 
				item.GetTotalCost(amountToBuy));
        }
    }

}
