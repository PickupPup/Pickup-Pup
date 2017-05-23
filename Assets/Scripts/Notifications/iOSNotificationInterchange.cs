/*
 * Author(s): Isaiah Mann
 * Description: Sends local notifications on iOS
 * Usage: [no notes]
 */

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
		LocalNotification iOSNotification;
		if(register.TryGetNotification(notification, out iOSNotification))
		{
			NotificationServices.CancelLocalNotification(iOSNotification);
		}
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
	Dictionary<int, LocalNotification> notificationLookup = new Dictionary<int, LocalNotification>();

	internal void CleanupOldNotifications()
	{
		Dictionary<int, LocalNotification> deepCopy = new Dictionary<int, LocalNotification>(notificationLookup);
		foreach(int id in deepCopy.Keys)
		{
			if(deepCopy[id].fireDate.CompareTo(DateTime.Now) < k.NONE_VALUE)
			{
				notificationLookup.Remove(id);
			}
		}
	}

	internal LocalNotification RegisterNotification(PPNotification notification)
	{
		LocalNotification iosNotification = createNotification(notification);
		notificationLookup[notification.GetHashCode()] = iosNotification;
		return iosNotification;
	}

	internal bool DestroyNotification(PPNotification notification)
	{
		return notificationLookup.Remove(notification.GetHashCode());
	}
		
	internal bool TryGetNotification(PPNotification notification, out LocalNotification iOSNotification)
	{
		return notificationLookup.TryGetValue(notification.GetHashCode(), out iOSNotification);
	}
		
	LocalNotification createNotification(PPNotification notification)
	{
		LocalNotification iOSNotification = new LocalNotification();
		iOSNotification.alertAction = notification.Title;
		iOSNotification.alertBody = notification.Body;
		iOSNotification.fireDate = notification.FireDate;
		return iOSNotification;
	}

}