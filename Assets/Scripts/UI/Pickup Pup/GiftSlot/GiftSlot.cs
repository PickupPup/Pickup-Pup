/*
 * Authors: Grace Barrett-Snyder, James Hostetler
 * Description: Parent class for a Gift's thumbnail.
 */

using UnityEngine;
using UnityEngine.UI;

public class GiftSlot : PPUIElement 
{
    protected GiftItem giftInfo;
    Image giftImage;

    // Initializes a gift by setting component references and displaying its sprite.
    public void Init(GiftItem gift, Sprite giftSprite)
    {
		this.giftInfo = gift;
		giftImage = GetComponentInChildren<Image>();

		//TEMP, Grabs generic image.
		giftSprite = giftImage.sprite;
		setSlot(this.giftInfo, giftSprite);
    }
	
    // Sets the gift's sprite.
	void setSlot(GiftItem gift, Sprite giftSprite)
    {
        giftImage.sprite = giftSprite;
    }


}
