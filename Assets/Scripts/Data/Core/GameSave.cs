/*
 * Author: Isaiah Mann
 * Description: Game save
 */

using System;
using System.Runtime.Serialization;

[Serializable]
public abstract class GameSave : SerializableData, ISerializable
{
	#region Instance Acessors 

	public string SaveID 
	{
		get
		{
			if(saveID == default(Guid))
			{
				saveID = new Guid();
			}
			return saveID.ToString();
		}
	}

	public DateTime TimeStamp 
	{
		get;
		private set;
	}

	public float TimeInSecSinceLastSave
	{
		get
		{
			return (float) (DateTime.Now - this.TimeStamp).TotalSeconds;
		}
	}

	#endregion

	Guid saveID;

	protected GameSave()
	{
		this.TimeStamp = DateTime.Now;
		this.saveID = new Guid();
	}

	#region ISerializable interface

	// The special constructor is used to deserialize values.
	public GameSave(SerializationInfo info, StreamingContext context)
	{
		TimeStamp = (DateTime) info.GetValue(TIME_STAMP, typeof(DateTime));
		saveID = (Guid) info.GetValue(SAVE_ID, typeof(Guid));
	}
		
	// Implement this method to serialize data. The method is called on serialization.
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(TIME_STAMP, DateTime.Now);
		info.AddValue(SAVE_ID, saveID);
	}

	#endregion

}
