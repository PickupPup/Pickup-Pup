/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using System.Collections.Generic;

public class AnalyticsEvent : DataEvent
{
	#region Instance Accessors

	public Dictionary<string, object> Properties
	{
		get
		{
			return this.properties;
		}
	}

	#endregion

	Dictionary<string, object> properties;

	public AnalyticsEvent(string id, string[] propertyKeys, object[] propertyVals)
	{
		this.id = id;
		this.properties = new ParallelArray<string, object>(propertyKeys, propertyVals).ToDict();
	}

}
