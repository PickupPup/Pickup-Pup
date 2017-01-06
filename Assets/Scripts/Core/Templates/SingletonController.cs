/*
 * Author: Isaiah Mann
 * Description: Provides generic behaviour for a singleton MonoBehaviourExtended class
 */

using UnityEngine;
using System.Collections;
using System.Reflection;

public class SingletonController<T> : Controller where T : class 
{
	#region Static Accessors

	public static T Instance 
	{
		get 
		{
			return _instance;
		}
	}

	#endregion
		
	protected static bool hasInstance 
	{
		get 
		{
			return _instance != null;
		}
	}

	protected static T _instance;

	protected bool isSingleton 
	{
		get 
		{
			return Instance.Equals(this);
		}
	}

	protected bool dontDestroyOnLoad = false;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		if(tryInit(ref _instance, this as T, gameObject, dontDestroyOnLoad)) 
		{
			base.setReferences();
		}
	}

	protected override void cleanupReferences() 
	{
		base.cleanupReferences();
		tryCleanupSingleton(ref _instance, this as T);
	}

	#endregion

	bool tryInit(ref T singleton, T instance, GameObject gameObject, bool dontDestroyOnLoad = false)
	{
		if(singleton == null) 
		{
			singleton = instance;
			if(dontDestroyOnLoad) 
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			return true;
		} 
		else 
		{
			Object.Destroy(gameObject);
			return false;
		}
	}

	bool tryCleanupSingleton(ref T singleton, T instance) 
	{
		if(singleton == instance)
		{
			singleton = default(T);
			return true;
		} 
		else 
		{
			return false;
		}
	}

}
