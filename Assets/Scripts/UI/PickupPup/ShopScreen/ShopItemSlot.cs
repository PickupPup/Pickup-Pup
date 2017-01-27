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
    Text priceText;
    [SerializeField]
    Image itemImage;

    PPShopUIController shop;
    ShopItem item;

    public void Init(PPShopUIController shop, ShopItem item)
    {
        this.shop = shop;
        this.item = item;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
        priceText.text = item.CostStr;
        // TODO: Set item Image
    }

    public void Buy()
    {
        gameController.TryBuyItem(item);
    }

}
