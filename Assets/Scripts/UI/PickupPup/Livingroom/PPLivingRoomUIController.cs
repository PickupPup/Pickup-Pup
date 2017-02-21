/*
 * Author: James Hostetler
 * Description: Controls the Livingroom UI.
 */

using UnityEngine;

public class PPLivingRoomUIController : PPUIController
{
    [SerializeField]
    RedeemDisplay rDisplay;

    GiftItem[] gifts;
    GiftRedeemSlot[] giftSlots;
    GiftDatabase giftBase;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        promptID = PromptID.ScoutingPrompt;
        base.setReferences();
        giftSlots = GetComponentsInChildren<GiftRedeemSlot>(); 
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadLivingRoom);

        giftBase = gameController.Gifts;
        giftBase.Initialize();
        gifts = giftBase.Gifts;
        generateGift(gifts);
    }

    #endregion

    #region PPUIController Overrides

    protected override void startTutorial()
    {
        LivingRoomTutorial livingRoomTutorial = (LivingRoomTutorial) tutorial;
        livingRoomTutorial.GetComponent<UICanvas>().Show();
        livingRoomTutorial.StartTutorial();
    }

    #endregion

    // TEMPORARY
    void generateGift(GiftItem[] gifts)
    {
        for (int i = 0; i < giftSlots.Length; i++)
        {
            GiftRedeemSlot giftSlot = giftSlots[i];
            giftSlot.Init(this, gifts[Random.Range(0, gifts.Length)]);
        }
    }

    public void RedeemGift(GiftItem gift)
    {
        rDisplay.Show();
        rDisplay.UpdateDisplay(gift, this);
    }

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

    public void OnShopClick()
    {
        sceneController.LoadShop();
    }

}
