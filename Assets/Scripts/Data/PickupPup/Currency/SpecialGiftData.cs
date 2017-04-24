/*
 * Author(s): Isaiah Mann
 * Description: Special Object currency
 * Usage: [no notes]
 */

using k = PPGlobal;

[System.Serializable]
public abstract class SpecialGiftData : CurrencyData
{
	[System.NonSerialized]
	protected DogDescriptor finderDog = DogDescriptor.Default();

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

	public void SetFinder(DogDescriptor dog)
	{
		this.finderDog = dog;
	}

}
