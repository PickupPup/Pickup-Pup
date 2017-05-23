/*
 * Author(s): Isaiah Mann
 * Description: Handles sending and scheduling notifications
 * Usage: [no notes]
 */

using System;

public class NotificationController : SingletonController<NotificationController>
{
	bool supportsNotifications
	{
		get
		{
			return notifications != null;
		}
	}

	INotificationInterchange notifications;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();

		#if UNITY_IOS

		notifications = new iOSNotificationInterchange();

		#endif

	}

	#endregion

	public void SendNotification(string title, string body, DateTime fireDate)
	{
		if(supportsNotifications)
		{
			notifications.SendNotification(new PPNotification(title, body, fireDate));
		}
	}

}
