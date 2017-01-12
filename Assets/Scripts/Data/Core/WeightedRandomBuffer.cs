/*
 * Author(s): Isaiah Mann
 * Description: Each option has a certain odds of being picked
 * Usage: Source and Weight Arrays Should be Same Length
 */

public class WeightedRandomBuffer<T> : RandomBuffer<T>
{
	const int DEFAULT_PRECISION = 100;

	float[] weights;
	int precision;
	int[] indexesAsOdds;

	public WeightedRandomBuffer(T[] source, float[] weights, int precision = DEFAULT_PRECISION):
	base(source)
	{
		this.weights = weights;
		this.precision = precision;
		setupOdds();
	}

	void setupOdds()
	{
		float sum = 0;
		foreach(float weight in weights)
		{
			sum += weight;
		}
		int[] weightsAsIndexCounts = new int[weights.Length];
		for(int i = 0; i < weightsAsIndexCounts.Length; i++)
		{
			int count = (int) ((float) precision * (weights[i] / sum));
			weightsAsIndexCounts[i] = count;
		}
		indexesAsOdds = new int[precision];
		int currentCount = 0;
		int currentIndex = 0;
		for(int i = 0; i < indexesAsOdds.Length; i++)
		{
			if(currentIndex >= weightsAsIndexCounts.Length)
			{
				break;
			}
			if(currentCount < weightsAsIndexCounts[currentIndex])
			{
				indexesAsOdds[i] = currentIndex;
				currentCount++;
			}
			else
			{
				currentCount = 0;
				currentIndex++;
			}
		}
	}

	protected override int nextRandomIndex ()
	{
		return indexesAsOdds[random.Next(precision)];
	}

}
