/*
 * Author: Isaiah Mann
 * Description: Handles Data persistence
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public abstract class DataController : SingletonController<DataController> 
{
	protected bool hasSaveBuffer 
	{
		get 
		{
			return saveBuffer != null;
		}
	}
		
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

}
