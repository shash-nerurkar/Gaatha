using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  // VARIABLES
  [SerializeField] private Sound[] sounds;
  private static SoundManager _instance;
  public static SoundManager instance { get { return _instance; } }
  private List<int> pausedSoundIndices;


  void Awake() {
    pausedSoundIndices = new List<int>();

    if ( _instance != null && _instance != this ) {
      Destroy( this.gameObject );
    }
    else {
      _instance = this;
    }

    foreach ( Sound sound in sounds ) {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;

      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.isLoop;
    }

    PausePanel.OnGamePauseToggledAction += OnGamePauseToggled;
  }
  
  // FIND SOUND AND PLAY IT
  public void Play( string name ) {
    Sound sound = Array.Find( sounds, s => s.name == name );

    if ( sound == null )
      Debug.LogError("Sound " + name + " Not Found!");
    else
      sound.source.Play();
  }

  // FIND SOUND AND STOP IT
  public void Stop( string name ) {
    Sound sound = Array.Find( sounds, s => s.name == name );

    if ( sound == null )
      Debug.LogError("Sound " + name + " Not Found!");
    else
      sound.source.Stop();
  }

  // PAUSE ALL ONGOING SOUNDS / PLAY ALL ONGOING PAUSED SOUNDS
  void OnGamePauseToggled( bool isPaused ) {
    if( isPaused ) {
      pausedSoundIndices.Clear();

      for( int index = 0; index < sounds.Length; index++ )
        if( sounds[ index ].source.isPlaying ) {
          sounds[ index ].source.Pause();
          pausedSoundIndices.Add( index );
        }
    }
    else {
      foreach( int index in pausedSoundIndices )
        sounds[ index ].source.Play();
      
      pausedSoundIndices.Clear();
    }
  }

  void OnDestroy() {
      PausePanel.OnGamePauseToggledAction -= OnGamePauseToggled;
  }
}
