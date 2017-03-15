/*
 * Author(s): Isaiah Mann
 * Description: Generic class to control sending anayltics
 * Usage: [no notes]
 */

public abstract class AnalyticsController : SingletonController<AnalyticsController>
{
	// Generic interface used to connect to a particular service
	IAnalyticsInterchange interchange;

	public virtual void SendEvent(AnalyticsEvent targetEvent)
	{
		interchange.SendEvent(targetEvent);
	}

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
		this.interchange = getInterchange();
	}

	#endregion

	// Provides a way to pass events to the analytics service in question
	protected abstract IAnalyticsInterchange getInterchange();

}
