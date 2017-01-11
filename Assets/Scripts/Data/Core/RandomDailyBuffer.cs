/*
 * Author(s): Isaiah Mann
 * Description: Generates same random sequence each day
 * Usage: [no notes]
 */

using System;

public class RandomDailyBuffer<T> : RandomBuffer<T>
{
	public RandomDailyBuffer(T[] source) : base(source)
	{

	}

	#region RandomBuffer Overrides

	protected override void setupRandomFormula()
	{
		random = new Random(DateTime.Today.GetHashCode());	
	}

	public override void Refresh()
	{
		base.Refresh ();
		// Need to reseed formula to return to beginning of sequence
		setupRandomFormula();
	}

	#endregion

}
