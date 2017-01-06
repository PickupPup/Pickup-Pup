/*
 * Author(s): Isaiah Mann
 * Description: An abstract representation of audio data
 */

using UnityEngine;

[System.Serializable]
public abstract class AudioData : SerializableData
{	
	#region Instance Accessors

	public string Name
	{
		get 
		{
			return this.name;
		}
	}

	public string[] Events 
	{
		get 
		{
			return this.events;
		}
	}

	public string[] StopEvents
	{
		get
		{
			return this.stopEvents;
		}
	}

	#endregion

	[SerializeField]
	protected string type;

	protected AudioLoader loader;

	[SerializeField]
	string name;
	[SerializeField]
	string[] events;
	[SerializeField]
	string[] stopEvents;

	public abstract AudioFile GetNextFile();
	public abstract AudioFile GetCurrentFile();

	public virtual bool HasCurrentFile() 
	{
		return true;
	}

	public void SetLoader(AudioLoader loader)
	{
		this.loader = loader;
	}

}
