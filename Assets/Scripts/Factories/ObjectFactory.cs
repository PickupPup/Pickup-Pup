/*
 * Author(s): Isaiah Mann
 * Description: Creates instances of an object (generic)
 */

[System.Serializable]
public abstract class ObjectFactory<T> : ResourceLoader
{
	public abstract T Create(params object[] args);
	public abstract T[] CreateGroup(params object[] args);

}
