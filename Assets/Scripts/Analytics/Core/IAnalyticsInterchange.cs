/*
 * Author(s): Isaiah Mann
 * Description: Used to send an event to a particular analytics service
 * Usage: Should have an implementation for each potential analytics service in use
 */

public interface IAnalyticsInterchange 
{
	void SendEvent(AnalyticsEvent targetEvent);
    void UpdateProperties(PPGameSave gameSave);

}
