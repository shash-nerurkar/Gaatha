using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private static SoundManager _instance;
  public Sound[] sounds;

  public static SoundManager instance { get { return _instance; } }

  void Awake() {
    if (_instance != null && _instance != this) {
      Destroy(this.gameObject);
    }
    else {
      _instance = this;
    }

    foreach (Sound sound in sounds)
    {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;

      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.isLoop;
    }
  }
  
  // FIND SOUND AND PLAY IT
  public void Play(string name) {
    Sound sound = Array.Find(sounds, s => s.name == name);

    if (sound == null)
      Debug.LogError("Sound " + name + " Not Found!");
    else
      sound.source.Play();
  }

  // FIND SOUND AND STOP IT
  public void Stop(string name)
  {
    Sound sound = Array.Find(sounds, s => s.name == name);

    if (sound == null)
      Debug.LogError("Sound " + name + " Not Found!");
    else
      sound.source.Stop();
  }
}
