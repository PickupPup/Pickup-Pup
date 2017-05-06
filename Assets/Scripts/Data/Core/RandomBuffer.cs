/*
 * Author: Isaiah Mann
 * Description: Continues to fetch random elements from the array
 */

using System;
using System.Collections;
using System.Collections.Generic;

public class RandomBuffer<T> 
{	
	// Using IList Interface allows for any data structure w/ a count and indexed accessor
	protected virtual IList<T> source 
	{
		get 
		{
			return backingSource;
		}
	}

	protected Random random;

	T[] backingSource;
	// Using a hash set for fast lookup
	HashSet<int> usedIndices;

	public RandomBuffer(T[] source, bool setupHandledInSubclass = false) 
	{
		this.backingSource = source;
		if(!setupHandledInSubclass)
		{
			setup();
		}
	}

	protected RandomBuffer()
	{
		setup();
	}

	protected virtual void setup()
	{
		setupRandomFormula();
		usedIndices = new HashSet<int>();
	}

	protected virtual void setupRandomFormula()
	{
		random = new Random();
	}

	// Adapted from: http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions
	protected virtual int nextRandomIndex() 
	{
		// Avoids creating an infinite loop by dumping all of the used indices (once all indices have been used)
		if(needsRefresh()) 
		{
			Refresh();
		}
		int index;
		do 
		{
			index = random.Next(source.Count);
		}
		while(usedIndices.Contains(index));
		usedIndices.Add(index);
		return index;
	}

	public bool HasNext()
	{
		return !needsRefresh();
	}

	public T GetRandom() 
	{
		return source[nextRandomIndex()];
	}

	public T[] GetRandom(int count) 
	{
		T[] randomList = new T[count];
		for(int i = 0; i < count; i++) 
		{
			randomList[i] = GetRandom();
		}
		return randomList;
	}

	bool needsRefresh() 
	{
		return usedIndices.Count >= source.Count;
	}
			
	public virtual void Refresh() 
	{
		usedIndices.Clear();
	}

}
