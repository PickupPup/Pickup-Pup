/*
 * Author(s): Isaiah Mann
 * Description: Sends local notifications on iOS
 * Usage: [no notes]
 */

using System;
using UnityEngine.iOS;

[Serializable]
public class iOSNotificationInterchange : INotificationInterchange
{
	#region INotificationInterchange Interface

	void INotificationInterchange.SendNotification(PPNotification notification)
	{
		
	}

	void INotificationInterchange.CancelNotification(PPNotification notification)
	{

	}
		
	#endregion

}
