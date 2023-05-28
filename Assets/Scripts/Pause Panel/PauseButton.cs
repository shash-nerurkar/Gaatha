using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    // COMPONENTS
    private Button button;
    private Image image;
    
    // VARIABLES
    [SerializeField] private Sprite buttonSpritePaused;
    [SerializeField] private Sprite buttonSpriteUnpaused;
    private bool isPaused = false;

    // ACTIONS
    public static event Action PauseGameToggleAction;

    void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        PausePanel.OnGamePauseToggledAction += SetPauseStatus;

        button?.onClick.AddListener( delegate { 
            PauseGameToggleAction?.Invoke();
        });
    }

    void SetPauseStatus( bool status ) {
        isPaused = status;

        if( isPaused ) {
            image.sprite = buttonSpritePaused;
        }
        else {
            image.sprite = buttonSpriteUnpaused;
        }
    }

    void OnDestroy() {
        PausePanel.OnGamePauseToggledAction -= SetPauseStatus;
    }
}
