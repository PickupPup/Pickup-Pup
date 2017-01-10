/*
 * Author: Isaiah Mann
 * Description: Handles Data persistence
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;

#endif

public abstract class DataController : SingletonController<DataController> 
{
	protected bool hasSaveBuffer 
	{
		get 
		{
			return saveBuffer != null;
		}
	}

	#if UNITY_EDITOR

	[Header("(Reset can only be called in-Editor)")]
	[SerializeField]
	KeyCode resetDataKey;

	#endif

	string filePath;
	SerializableData saveBuffer;

	public void Buffer(SerializableData file) 
	{
		this.saveBuffer = file;
	}

	public bool HasSaveFile() 
	{
		return FileUtil.FileExistsAtPath(filePath);
	}

	public void SetFilePath(string filePath) 
	{
		this.filePath = filePath;
	}

	public SerializableData Load() 
	{
		if(HasSaveFile()) 
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.Open);
			SerializableData data = (SerializableData) binaryFormatter.Deserialize(file);
			file.Close();
			this.saveBuffer = data;
			return data;
		}
		else 
		{
			this.saveBuffer = getDefaultFile();
			return this.saveBuffer;
		}
	}

	public bool Save() 
	{
		if(hasSaveBuffer) 
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
			binaryFormatter.Serialize(file, saveBuffer);
			file.Close();
			return true;
		} 
		else 
		{
			return false;
		}
	}

	public virtual void Reset() 
	{
		Buffer(getDefaultFile());
		Save();
	}

	protected abstract SerializableData getDefaultFile();

	#region Editor Debugging 

	#if UNITY_EDITOR

	void Update()
	{
		if(Input.GetKeyDown(resetDataKey))
		{
			Reset();
			// Reloads the scene to ensure data resets
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	#endif

	#endregion

}
