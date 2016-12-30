/*
 * Author: Isaiah Mann
 * Description: Handles Data persistence
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public abstract class DataController : SingletonController<DataController> {
	string filePath;
	SerializableData saveBuffer;
	protected bool hasSaveBuffer {
		get {
			return saveBuffer != null;
		}
	}

	public void Buffer (SerializableData file) {
		this.saveBuffer = file;
	}

	public bool HasSaveFile () {
		return FileUtil.FileExistsAtPath(filePath);
	}

	public void SetFilePath (string filePath) {
		this.filePath = filePath;
	}

	public SerializableData Load () {
		if (HasSaveFile()) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.Open);
			SerializableData data = (SerializableData) binaryFormatter.Deserialize(file);
			file.Close();
			this.saveBuffer = data;
			return data;
		} else {
			this.saveBuffer = getDefaultFile();
			return this.saveBuffer;
		}
	}

	public bool Save () {
		if (hasSaveBuffer) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
			binaryFormatter.Serialize(file, saveBuffer);
			file.Close();
			return true;
		} else {
			return false;
		}
	}

	protected abstract SerializableData getDefaultFile();
}
