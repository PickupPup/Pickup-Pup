/*
 * Author: Isaiah Mann
 * Description: Random buffer with a list backing structure instead of an array
 */

using System.Collections.Generic;

public class RandomListBuffer<T> : RandomBuffer<T>
{
	protected override IList<T> source
	{
		get
		{
			return backingSource;
		}
	}

	List<T> backingSource;

	public RandomListBuffer(IList<T> source)
	{
		backingSource = new List<T>(source);
	}

	public void AddToBuffer(T element) 
	{
		backingSource.Add(element);
	}

}
