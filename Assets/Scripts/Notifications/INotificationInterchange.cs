/*
 * Author(s): Isaiah Mann
 * Description: Interface for describing the interactions users can take with local notifications
 * Usage: [no notes]
 */

public interface INotificationInterchange
{
	void SendNotification(PPNotification notification);
	void CancelNotification(PPNotification notification);

}
