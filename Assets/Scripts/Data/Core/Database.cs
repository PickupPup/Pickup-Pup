/*
 * Author(s): Isaiah Mann
 * Description: A generic singleton style database
 */

[System.Serializable]
public abstract class Database<T> : ResourceLoader where T : class, new()
{
	#region Static Accessors

	public static T Instance 
	{	
		get 
		{
			if(_instance == null) 
			{
				_instance = new T();
			}
			return _instance;
		}
	}

	#endregion

	public bool IsInitialized
	{
		get
		{
			return this.isInitialized;
		}
	}

	static T _instance;

	bool isInitialized;

	protected Database() 
	{
		// NOTHING
	}

	public virtual void Initialize()
	{
		isInitialized = true;
	}

	public static void AssignInstance(T instance)
	{
		_instance = instance;
	}

	// Returns false if the datbase has already been initialized
	public virtual bool TryInit()
	{
		if(isInitialized)
		{
			return false;
		}
		else
		{
			Initialize();
			return true;
		}
	}
}
