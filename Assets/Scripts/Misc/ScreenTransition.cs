using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    // COMPONENTS
    private Material screenTransitionMaterial;

    // VARIABLES
    private float transitionTime = 1f;
    private string propertyName = "_Progress";
    private bool isGamePaused = false;

    // EVENTS
    public static event Action OnTransitionDone;

    private static ScreenTransition _instance;
    public static ScreenTransition instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        screenTransitionMaterial = GetComponent<Image>().material;

        PausePanel.OnGamePauseToggledAction += OnGamePauseToggled;
    }

    public void Show() => screenTransitionMaterial.SetFloat( propertyName, 0 );

    public void Hide() => screenTransitionMaterial.SetFloat( propertyName, 1 );

    public void FadeIn() => StartCoroutine( Transition(fadeIn: true) );

    public void FadeOut() => StartCoroutine( Transition(fadeIn: false) );

    public void OnGamePauseToggled( bool toggleStatus ) => isGamePaused = toggleStatus;

    private IEnumerator Transition( bool fadeIn ) {
        if(fadeIn) {
            float currentTime = transitionTime;
            while ( currentTime > 0 ) {
                currentTime -= isGamePaused ? Time.unscaledDeltaTime : Time.deltaTime;
                screenTransitionMaterial.SetFloat( propertyName, Mathf.Clamp01( currentTime / transitionTime ) );
                yield return null;
            }
        }
        else {
            float currentTime = 0;
            while ( currentTime < transitionTime ) {
                currentTime += isGamePaused ? Time.unscaledDeltaTime : Time.deltaTime;
                screenTransitionMaterial.SetFloat( propertyName, Mathf.Clamp01( currentTime / transitionTime ) );
                yield return null;
            }
        }
        OnTransitionDone?.Invoke();
    }
    
    void OnDestroy() {
        PausePanel.OnGamePauseToggledAction -= OnGamePauseToggled;
    }
}
