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

    PPHomeUIController room;

    #region GiftSlot Overrides

    public void Init(PPHomeUIController room, GiftItem gift)
    {
        this.room = room;
    }

    #endregion
	
    // Leads to Redeem Display. Turns off GiftSlot.
    public void Redeem()
    {
        if(room)
        {
            //room.RedeemGift(this.gift);
            gameObject.SetActive(false);
        }
    }

}
