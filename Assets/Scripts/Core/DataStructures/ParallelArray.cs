/*
 * Author(s): Isaiah Mann
 * Description: A class that joins to arrays as parallel (not truly jagged as each pair is closely coupled together)
 * Usage: [no notes]
 */

using System;
using System.Collections.Generic;

[Serializable]
public class ParallelArray<T, K>
{
	#region Instance Accessors

	// Shortcut allowing square bracket indexing into the data structure
	public DataPair<T, K> this[int index]
	{
		get
		{
			return Get(index);
		}
	}

	#endregion

	DataPair<T, K>[] data;

	public ParallelArray(T[] first, K[] second)
	{
		// Error checking
		if(first.Length != second.Length)
		{
			throw new Exception(
				string.Format("Array lengths {0} and {1} are not equal", 
					first.Length, 
					second.Length));
		}

		this.data = generatePairs(first, second);
	}

	public DataPair<T, K> Get(int index)
	{
		return data[index];
	}

	public void Set(int index, T first, K second)
	{
		data[index] = new DataPair<T, K>(first, second);
	}

	public Dictionary<T, K> ToDict()
	{
		Dictionary<T, K> dict = new Dictionary<T, K>();
		foreach(DataPair<T, K> pair in data) 
		{
			if(dict.ContainsKey(pair.First)) 
			{
				continue;
			}
			else 
			{
				dict.Add(pair.First, pair.Second);
			}
		}
		return dict;
	}

	public Dictionary<K, T> ToReverseDict() 
	{
		Dictionary<K, T> dict = new Dictionary<K, T>();
		foreach(DataPair<T, K> pair in data) 
		{
			if(dict.ContainsKey(pair.Second)) 
			{
				continue;
			}
			else 
			{
				dict.Add(pair.Second, pair.First);
			}
		}
		return dict;
	}

	DataPair<T, K>[] generatePairs(T[] first, K[] second)
	{
		int length = getLength(first, second);
		DataPair<T, K>[] data = new DataPair<T, K>[length];
		for(int i = 0; i < length; i++)
		{
			data[i] = new DataPair<T, K>(first[i], second[i]);
		}
		return data;
	}

	// Failsafe to prevent OutOfRange exception if one array is longer
	int getLength(T[] first, K[] second)
	{
		return Math.Min(first.Length, second.Length);
	}

}

public class DataPair<T, K>
{
	#region Instance Acessors 

	public T First
	{
		get
		{
			return first;
		}
	}

	public K Second
	{
		get 
		{
			return second;
		}
	}

	#endregion

	T first;
	K second;

	public DataPair(T first, K second)
	{
		this.first = first;
		this.second = second;
	}

}
