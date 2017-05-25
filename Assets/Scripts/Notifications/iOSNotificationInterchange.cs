/*
 * Author(s): Isaiah Mann
 * Description: Sends local notifications on iOS
 * Usage: [no notes]
 */

#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.iOS;
using k = PPGlobal;

public class iOSNotificationInterchange : INotificationInterchange
{
	const string REGISTER_FILE_NAME = "iOSNotifications.dat";

	string registerPath;
	iOSNotificationRegister register;

	public iOSNotificationInterchange(string registerPath)
	{
		NotificationServices.RegisterForNotifications(
			NotificationType.Alert | 
			NotificationType.Badge | 
			NotificationType.Sound);
		this.registerPath = registerPath;
		this.register = loadRegister();
	}
		
	~iOSNotificationInterchange()
	{
		saveRegister();
	}

	#region INotificationInterchange Interface

	void INotificationInterchange.SendNotification(PPNotification notification)
	{
		LocalNotification iOSNotification = register.RegisterNotification(notification);
		NotificationServices.ScheduleLocalNotification(iOSNotification);
	}

	void INotificationInterchange.CancelNotification(PPNotification notification)
	{
		register.DestroyNotification(notification);
	}
		
	#endregion

	iOSNotificationRegister loadRegister()
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(getRegisterFilePath(), FileMode.Open);
			iOSNotificationRegister register = (iOSNotificationRegister) formatter.Deserialize(file);
			file.Close();
			return register;
		}
		catch
		{
			return new iOSNotificationRegister();
		}
	}

	void saveRegister()
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(getRegisterFilePath(), FileMode.OpenOrCreate);
			formatter.Serialize(file, register);
			file.Close();
		}
		catch(Exception e)
		{
			UnityEngine.Debug.LogError(e);
		}
	}

	string getRegisterFilePath()
	{
		return Path.Combine(registerPath, REGISTER_FILE_NAME);
	}

}

[Serializable]
internal class iOSNotificationRegister
{

	internal LocalNotification RegisterNotification(PPNotification notification)
	{
		LocalNotification iosNotification = createNotification(notification);
		return iosNotification;
	}

	internal bool DestroyNotification(PPNotification notification)
	{
		foreach(LocalNotification iOSNotification in NotificationServices.localNotifications)
		{
			if(notificationMatch(notification, iOSNotification))	
			{
				NotificationServices.CancelLocalNotification(iOSNotification);
				return true;
			}
		}
		return false;
	}
		
	LocalNotification createNotification(PPNotification notification)
	{
		LocalNotification iOSNotification = new LocalNotification();
		iOSNotification.alertAction = notification.Title;
		iOSNotification.alertBody = notification.Body;
		iOSNotification.fireDate = notification.FireDate;
		return iOSNotification;
	}

	bool notificationMatch(PPNotification notification, LocalNotification iOSNotification)
	{
		return notification.Title.Equals(iOSNotification.alertAction) &&
			notification.Body.Equals(iOSNotification.alertBody) &&
			notification.FireDate.Equals(iOSNotification.fireDate);
	}

}

#endif
