/*
 * Author: Isaiah Mann
 * Desc: Abstract data class
 */

[System.Serializable]
public abstract class SerializableData 
{

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
