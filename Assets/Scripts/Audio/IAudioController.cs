/*
 * Author(s): Isaiah Mann
 * Description: Describes the public functioanlity of the AudioController system
 */

public interface IAudioController {
     void Play (AudioFile file);
     void Stop (AudioFile file);
     void ToggleFXMute();
     void ToggleMusicMute();
}
