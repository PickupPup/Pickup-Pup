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

	static T _instance;

	protected Database() 
	{
		// NOTHING	
	}

	public abstract void Initialize();

	public static void AssignInstance(T instance)
	{
		_instance = instance;
	}

}
