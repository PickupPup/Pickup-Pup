/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image itemImage;
	[SerializeField]
	Text amountText;

    [SerializeField]
    PriceTag priceTag;
    Button button;

    PPShopUIController shop;
    ShopItem item;

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

    public void Init(PPShopUIController shop, ShopItem item)
    {
        subscribeEvents();
        this.shop = shop;
        this.item = item;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
		if(amountText)
		{
			amountText.text = item.Value.ToString();
		}
        priceTag.Set(item.Cost);
        priceTag.ShowPurchasable();
        itemImage.sprite = item.Icon;
    }

    public void Buy()
    {
        gameController.TryBuyItem(item);
    }

    void tryToggle(int amount)
    {
        if(button)
        {
            button.interactable = gameController.CanAfford(item.CostCurrencyType, item.Cost);
        }
    }

}
