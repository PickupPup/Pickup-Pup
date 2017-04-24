/*
 * Author(s): Isaiah Mann
 * Description: Special Object currency
 * Usage: [no notes]
 */

using k = PPGlobal;

[System.Serializable]
public abstract class SpecialGiftData : CurrencyData
{
	public SpecialGiftData(CurrencyType type) :
	base(type, k.SINGLE_VALUE)
	{
		// NOTHING
	}

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
