/*
 * Author: Isaiah Mann
 * Desc: Continues to fetch random elements from the array
 */

using System;
using System.Collections.Generic;

public class RandomBuffer<T> {	
	T[] source;
	Random random;
	// Using a hash set for fast lookup
	HashSet<int> usedIndices;

	public RandomBuffer (T[] source) {
		this.source = source;
		random = new Random();
		usedIndices = new HashSet<int>();
	}

	// Adapted from: http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions
	int nextRandomIndex () {
		// Avoids creating an infinite loop by dumping all of the used indices (once all indices have been used)
		if (needsRefresh()) {
			refresh();
		}
		int index;
		do {
			index = random.Next(source.Length);
		} while (usedIndices.Contains(index));
		usedIndices.Add(index);
		return index;
	}

	public T GetRandom () {
		return source[nextRandomIndex()];
	}

	public T[] GetRandom (int count) {
		T[] randomList = new T[count];
		for (int i = 0; i < count; i++) {
			randomList[i] = GetRandom();
		}
		return randomList;
	}

	bool needsRefresh () {
		return usedIndices.Count >= source.Length;
	}
			
	void refresh () {
		usedIndices.Clear();
	}
}
