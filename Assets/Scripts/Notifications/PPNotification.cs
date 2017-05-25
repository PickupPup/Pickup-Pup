/*
 * Author(s): Isaiah Mann
 * Description: Stores a notification to be sent at a specified date
 * Usage: [no notes]
 */

using System;

[Serializable]
public class PPNotification : PPData
{
	#region Instance Accessors

	public string Title
	{
		get;
		private set;
	}

	public string Body
	{
		get;
		private set;
	}

	public DateTime FireDate
	{
		get;
		private set;
	}

	#endregion

	public PPNotification(string title, string body, DateTime fireDate)
	{
		this.Title = title;
		this.Body = body;
		this.FireDate = fireDate;
	}
		
	#region Object Overrides 

	public override int GetHashCode()
	{
		return Title.GetHashCode() + Body.GetHashCode() + FireDate.GetHashCode();
	}

	#endregion

}
