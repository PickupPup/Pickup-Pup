/*
 * Author: Isaiah Mann
 * Description: Abstract data class
 */

using k = PPGlobal;

[System.Serializable]
public abstract class SerializableData 
{
	protected const string CURRENCY = k.CURRENCY;
	protected const string ADOPTED = k.ADOPTED;
	protected const string SCOUTING = k.SCOUTING;
	protected const string TIME_STAMP = k.TIME_STAMP;
	protected const string DAILY_GIFT_COUNTDOWN = k.DAILY_GIFT_COUNTDOWN;
	protected const string FIRST_GIFT = k.FIRST_GIFT;

	const float FULL_PERCENT = k.FULL_PERCENT;

	protected float percentToDecimal(int percent)
	{
		return percentToDecimal((float) percent);
	}

	protected float percentToDecimal(float percent)
	{
		return percent / FULL_PERCENT;
	}

}
