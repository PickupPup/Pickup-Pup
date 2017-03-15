/*
 * Author(s): Isaiah Mann
 * Description: Abstract representation of a data class which can be passed as an event
 * Usage: [no notes]
 */

public abstract class DataEvent 
{
	#region Instance Accessors

	public string ID
	{
		get
		{
			return this.id;
		}
	}

	#endregion

	protected string id;

}
