/*
 * Author: James Hostetler
 * Description: Controls a GiftSlot on the Livingroom & Yard scene.
 */

using UnityEngine;
using UnityEngine.UI;

public class GiftRedeemSlot : GiftSlot 
{

    [SerializeField]
    Image itemImage;

    PPLivingroomUIController room;
    GiftItem gift;

    #region GiftSlot Overrides

    public void Init(PPLivingroomUIController room, GiftItem gift)
    {
        this.room = room;
        this.gift = gift;
    }

    #endregion
	
    //Leads to Redeem Display. Turns off GiftSlot.
    public void Redeem()
    {
        room.RedeemGift(this.gift);
        gameObject.SetActive(false);
    }

}
