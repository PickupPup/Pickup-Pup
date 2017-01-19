/*
 * Author: James Hostetler
 * Description: Controls a GiftSlot on the Living Room & Yard scene.
 */

using UnityEngine;
using UnityEngine.UI;

public class GiftRedeemSlot : GiftSlot 
{
    [SerializeField]
    Image itemImage;

    PPLivingRoomUIController room;
    GiftItem gift;

    #region GiftSlot Overrides

    public void Init(PPLivingRoomUIController room, GiftItem gift)
    {
        this.room = room;
        this.gift = gift;
    }

    #endregion
	
    // Leads to Redeem Display. Turns off GiftSlot.
    public void Redeem()
    {
        if (room)
        {
            room.RedeemGift(this.gift);
            gameObject.SetActive(false);
        }
    }

}
