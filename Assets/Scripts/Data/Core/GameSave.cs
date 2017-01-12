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

	protected GameSave()
	{
		this.TimeStamp = DateTime.Now;
	}

	#region ISerializable interface

	// The special constructor is used to deserialize values.
	public GameSave(SerializationInfo info, StreamingContext context)
	{
		TimeStamp = (DateTime) info.GetValue(TIME_STAMP, typeof(DateTime));
	}
		
	// Implement this method to serialize data. The method is called on serialization.
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(TIME_STAMP, DateTime.Now);
	}

	#endregion

}
