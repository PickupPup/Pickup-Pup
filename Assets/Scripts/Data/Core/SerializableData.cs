/*
 * Author: Isaiah Mann
 * Description: Abstract data class
 */

[System.Serializable]
public abstract class SerializableData 
{
	protected const string CURRENCY = "Currency";
	protected const string ADOPTED = "Adopted";
	protected const string SCOUTING = "Scouting";
	protected const string TIME_STAMP = "TimeStamp";

	const float FULL_PERCENT = 100f;

	protected float percentToDecimal(int percent)
	{
		return percentToDecimal((float) percent);
	}

	protected float percentToDecimal(float percent)
	{
		return percent / FULL_PERCENT;
	}

}
