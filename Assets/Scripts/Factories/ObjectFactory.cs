/*
 * Author(s): Isaiah Mann
 * Description: Creates instances of an object (generic)
 */

public abstract class ObjectFactory<T>
{
	public abstract T Create(params object[] args);
	public abstract T[] CreateGroup(params object[] args);

}
