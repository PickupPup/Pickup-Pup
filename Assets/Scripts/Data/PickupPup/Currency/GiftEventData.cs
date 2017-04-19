/*
 * Author(s): Isaiah Mann
 * Description: A special kind of dog gift with a narrative event tied to it
 * Usage: [no notes]
 */

[System.Serializable]
public class GiftEventData : SpecialGiftData
{
    public GiftEventData(int amount = 1) : base(CurrencyType.GiftEvent, amount){}

}
