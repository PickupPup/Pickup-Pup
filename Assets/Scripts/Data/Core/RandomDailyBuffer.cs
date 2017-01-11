/*
 * Author(s): Isaiah Mann
 * Description: Generates same random sequence each day
 * Usage: [no notes]
 */

using System;

public class RandomDailyBuffer<T> : RandomBuffer<T>
{
	// Use to simulate a different day than the current
	bool shouldOverrideDay;
	DateTime overrideDay;

	public RandomDailyBuffer(T[] source) : base(source)
	{
		
	}

	public RandomDailyBuffer(T[] source, DateTime day) :
	base(source, setupHandledInSubclass:true)
	{
		this.shouldOverrideDay = true;
		this.overrideDay = day;
		// Need to call here because superclass constructor runs first (before we set the day override)
		setup();
	}

	#region RandomBuffer Overrides

	protected override void setupRandomFormula()
	{	
		int seed;
		if(shouldOverrideDay)
		{
			seed = generateSeedForDay(overrideDay);
		}
		else 
		{
			seed = generateSeedForDay(DateTime.Today);
		}
		random = new Random(seed);
	}

	public override void Refresh()
	{
		base.Refresh();
		// Need to reseed formula to return to beginning of sequence
		setupRandomFormula();
	}

	#endregion

	private int generateSeedForDay(DateTime day)
	{
		return day.Year + day.Month + day.Day;
	}

}
