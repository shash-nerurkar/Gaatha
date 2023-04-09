using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private static SoundManager _instance;
  public Sound[] sounds;
  private static Dictionary<string, float> soundTimerDictionary;

  public static SoundManager instance
  {
    get
    {
      return _instance;
    }
  }

  private void Awake() {
    if (_instance != null && _instance != this) {
      Destroy(this.gameObject);
    }
    else {
      _instance = this;
    }

    soundTimerDictionary = new Dictionary<string, float>();

    foreach (Sound sound in sounds)
    {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;

      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.isLoop;

      if (sound.hasCooldown) {
        Debug.Log(sound.name);
        soundTimerDictionary[sound.name] = 0f;
      }
    }
  }
}
