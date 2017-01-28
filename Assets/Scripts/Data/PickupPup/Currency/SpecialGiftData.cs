/*
 * Author(s): Isaiah Mann
 * Description: Special Object currency
 * Usage: [no notes]
 */

[System.Serializable]
public class SpecialGiftData : CurrencyData
{
    public SpecialGiftData(int initialAmount):
    base(CurrencyType.SpecialGift, initialAmount)
    {
        // NOTHING
    }

    // TODO: Implement special behaviour for currency type

}
