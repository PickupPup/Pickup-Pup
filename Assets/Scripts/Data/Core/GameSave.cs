/*
 * Author: Isaiah Mann
 * Description: Game save
 */

using System;
using System.Runtime.Serialization;

[Serializable]
public abstract class GameSave : SerializableData
{
	#region Instance Acessors 

	public DateTime TimeStamp 
	{
		get;
		private set;
	}

	#endregion

	protected GameSave()
	{
		this.TimeStamp = DateTime.Now;
	}

}
