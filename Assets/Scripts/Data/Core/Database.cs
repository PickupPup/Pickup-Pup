/*
 * Author(s): Isaiah Mann
 * Description: A generic singleton style database
 */

[System.Serializable]
public class Database<T> where T : class, new() {
	static T _instance;
	public static T Instance {	
		get {
			if (_instance == null) {
				_instance = new T();
			}
			return _instance;
		}
	}

	public Database() {}

	public virtual void Initialize() {
		// NOTHING
	}

	public static void AssignInstance (T instance) {
		_instance = instance;
	}
}
