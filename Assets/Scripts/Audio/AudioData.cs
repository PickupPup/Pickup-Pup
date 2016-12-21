/*
 * Author(s): Isaiah Mann
 * Description: An abstract representation of audio data
 */

[System.Serializable]
public abstract class AudioData {
	public string Name;
	public string[] Events;
	public string[] StopEvents;
	public string Type;
	public abstract AudioFile GetNextFile ();
	public abstract AudioFile GetCurrentFile ();
	public virtual bool HasCurrentFile () {
		return true;
	}
}
