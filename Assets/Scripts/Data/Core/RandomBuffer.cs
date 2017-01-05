/*
 * Author: Isaiah Mann
 * Desc: Continues to fetch random elements from the array
 */

using System;
using System.Collections;
using System.Collections.Generic;

public class RandomBuffer<T> 
{	
	
	protected virtual IList<T> source 
	{
		get 
		{
			return backingSource;
		}
	}

	T[] backingSource;
	Random random;
	// Using a hash set for fast lookup
	HashSet<int> usedIndices;

	public RandomBuffer(T[] source) 
	{
		this.backingSource = source;
		setup();
	}

	protected RandomBuffer()
	{
		setup();
	}

	void setup()
	{
		random = new Random();
		usedIndices = new HashSet<int>();
	}

	// Adapted from: http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions
	int nextRandomIndex() 
	{
		// Avoids creating an infinite loop by dumping all of the used indices (once all indices have been used)
		if(needsRefresh()) 
		{
			refresh();
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
			
	void refresh() 
	{
		usedIndices.Clear();
	}

}
