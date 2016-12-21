/*
 * Author(s): Isaiah Mann
 * Description: Represents a group fo audio files (can be randomely sampled)
 */

[System.Serializable]
public class AudioGroup : AudioData {
	RandomizedQueue<AudioFile> fileQueue;
	AudioFile currentFile;

	public AudioGroup (params AudioFile[] files) {
		checkFileQueue();
		foreach (AudioFile file in files) {
			fileQueue.Enqueue(file);
		}
	}

	public void AddFile (AudioFile file) {
		checkFileQueue();
		fileQueue.Enqueue(file);
	}

	void checkFileQueue () {
		if (fileQueue == null) {
			fileQueue = new RandomizedQueue<AudioFile>();
		}
	}

	public AudioFile GetRandomFile () {
		checkFileQueue();
		return (currentFile = fileQueue.Cycle());
	}

	public override AudioFile GetCurrentFile () {
		return currentFile;
	}

	public override AudioFile GetNextFile () {
		return GetRandomFile();
	}

	public override bool HasCurrentFile () {
		if (currentFile == null) {
			return false;
		} else {
			return base.HasCurrentFile ();
		}
	}
}
