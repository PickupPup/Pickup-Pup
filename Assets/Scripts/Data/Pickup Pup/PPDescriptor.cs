/*
 * Author(s): Isaiah Mann
 * Description: Descripts are data classes that associate with a MonoBehaviour to tell them what type of data they contain
 */

[System.Serializable]
public abstract class PPDescriptor : PPData 
{
	public PPDescriptor(DogDatabase data) : base(data)
	{
		// NOTHING
	}
}
