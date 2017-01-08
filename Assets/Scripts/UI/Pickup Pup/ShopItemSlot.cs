/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : UIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text priceText;
    [SerializeField]
    Image itemImage;

    ShopItem item;

    public void Init(ShopItem item)
    {
        this.item = item;
        nameText.text = item.ItemName;
        priceText.text = item.CostStr;
        // TODO: Set item
    }

}
