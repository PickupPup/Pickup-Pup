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

    public SpecialGiftData(CurrencyType type, int initialAmount):
    base(type, initialAmount)
    {
        // NOTHING
    }

    // TODO: Implement special behaviour for currency type

}
