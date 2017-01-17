/*
 * Author: Isaiah Mann 
 * Description: Wrapper class to extend the default behaviour of MonoBehaviours
 */

using UnityEngine;
using System.Collections;
using k = Global;

public abstract class MonoBehaviourExtended : MonoBehaviour, System.IComparable 
{
	protected bool referencesSet = false;
	protected bool referencesFetched = false;

	protected const int NONE_VALUE = k.NONE_VALUE;
	protected static int INVALID_VALUE = k.INVALID_VALUE; 

	public delegate void MonoAction();
	public delegate void MonoActionf(float monoFloat);
	public delegate void MonoActionInt(int monoInt);

	IEnumerator moveCoroutine;
	bool destroyOnNextLoad = false;

	#region Unity Methods

	void Awake() 
	{
		if(!referencesSet)
		{
			setReferences();
		}
		subscribeEvents();
	}

	void Start()
	{
		if(!referencesFetched)
		{
			fetchReferences();
		}
	}

	void OnDestroy() 
	{
		cleanupReferences();
		unsubscribeEvents();
		StopAllCoroutines();
	}

	void OnLevelWasLoaded(int level) 
	{
		handleSceneLoaded(level);
	}

	void OnApplicationQuit()
	{
		handleGameQuit();
	}

	void OnApplicationPause(bool isPaused)
	{
		handleGameTogglePause(isPaused);

		// iOS Does not produce calls to OnApplicationQuit, so treat all pauses as quit events
		#if UNITY_IOS

		if(isPaused)
		{
			handleGameQuit();
		}

		#endif
	}

	#endregion

	public virtual void Destroy()
	{
		Destroy(gameObject);
	}

	// Value should only be null if you're setting a trigger
	public bool QueryAnimator(AnimParam param, string key, object value = null) 
	{
		Animator animator = GetComponent<Animator>();
		if(animator == null) 
		{
			return false;
		}
		else 
		{
			try
			{
				switch(param) 
				{
					case AnimParam.Bool:
						animator.SetBool(key, (bool) value);
						return true;
					case AnimParam.Float:
						animator.SetFloat(key, (float) value);
						return true;
					case AnimParam.Int:
						animator.SetInteger(key, (int) value);
						return true;
					case AnimParam.Trigger:
						animator.SetTrigger(key);
						return true;
					default:
						return false;
				}
			} 
			catch 
			{
				return false;
			}
		}
	}

	public bool QuerySpriteRenderer(Sprite sprite) 
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if(renderer == null) 
		{
			return false;
		}
		else 
		{
			renderer.sprite = sprite;
			return true;
		}
	}

	protected virtual void subscribeEvents() 
	{
		EventController.Subscribe(handleNamedEvent);
	}

	protected virtual void unsubscribeEvents() 
	{
		EventController.Unsubscribe(handleNamedEvent);
	}

	protected virtual void setReferences() 
	{
		this.referencesSet = true;
	}

	protected virtual void fetchReferences() 
	{
		this.referencesFetched = true;
	}
		
	protected virtual void checkReferences()
	{
		if(!this.referencesSet)
		{
			this.setReferences();
		}
		if(!this.referencesFetched)
		{
			this.fetchReferences();
		}
	}

	protected virtual void cleanupReferences() 
	{
		// NOTHING
	}

	protected virtual void handleNamedEvent(string eventName) 
	{
		// NOTHING
	}

	protected virtual void handleSceneLoaded(int sceneIndex) 
	{
		if(destroyOnNextLoad) 
		{
			Destroy(gameObject);
		}
	}

	protected virtual void handleGameTogglePause(bool isPaused)
	{
		// NOTHING
	}

	protected virtual void handleGameQuit()
	{
		// NOTHING
	}

	protected virtual void markForDestroyOnLoad() 
	{
		destroyOnNextLoad = true;
	}

	public int CompareTo(object other) 
	{
		if(other is MonoBehaviourExtended) 
		{
			return this == (other as MonoBehaviourExtended) ? 0 : -1;
		} 
		else 
		{
			return -1;
		}
	}
		
	protected void moveTo(Vector3 targetPosition, float time, MonoAction callBack = null) 
	{
		haltMoveTo();
		moveCoroutine = linearLerp(transform, targetPosition, time, callBack);
		StartCoroutine(moveCoroutine);
	}

	protected void haltMoveTo() 
	{
		if(moveCoroutine != null) 
		{
			StopCoroutine(moveCoroutine);
		}
	}

	protected IEnumerator linearLerp(Transform transform, Vector3 targetPosition, float totalTime, MonoAction callBack = null) 
	{
		float timer = 0;
		Vector3 startPosition = transform.position;
		while(timer <= totalTime) 
		{
			transform.position = Vector3.Lerp(startPosition, targetPosition, timer);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = targetPosition;
		if(callBack != null)
		{
			callBack();
		}
	}

	protected void scale(Transform transform, float scaleFactor) 
	{
		transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
	}

	protected void scale(float scaleFactor) 
	{
		scale(transform, scaleFactor);
	}

	protected TextAsset loadTextAssetInResources(string pathInResources)
	{
		return Resources.Load<TextAsset>(pathInResources);
	}

	// Starts an arbitrary amount of coroutines
	protected void startCoroutines(params IEnumerator[] coroutines)
	{
		for(int i = 0; i < coroutines.Length; i++) 
		{
			StartCoroutine(coroutines[i]);
		}
	}

	// Source: http://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
	protected int mod(int rawValue, int modulus) 
	{
		return (rawValue % modulus + modulus) % modulus;
	}
		
	protected T ensureReference<T>(bool searchChildren = false) where T:Component
	{
		T component;
		if(searchChildren)
		{
			component = GetComponentInChildren<T>();
		}
		else 
		{
			component = GetComponent<T>();
		}
		if(component)
		{
			return component;
		}
		else
		{
			return gameObject.AddComponent<T>();
		}
	}

	protected T parseFromJSONInResources<T>(string pathInResoures)
	{
		TextAsset json = Resources.Load<TextAsset>(pathInResoures);
		return JsonUtility.FromJson<T>(json.text);
	}

}
