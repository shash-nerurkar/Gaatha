using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    // CHILDREN
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    
    // VARIABLES
    private bool isPanelActive = false;

    // ACTIONS
    public static event Action ShowSettingsPanelAction;
    public static event Action<bool> OnGamePauseToggledAction;
    public static event Action<SceneBuildIndex> GoToMenuAction;
    
    void Awake() {
        PauseButton.PauseGameToggleAction += SwitchPanelStates;

        // SHOW SETTINGS PANEL ON SETTINGS BUTTON CLICK
        settingsButton?.onClick.AddListener( delegate { 
            ShowSettingsPanelAction?.Invoke();
        });

        // GO TO MENU ON HOME BUTTON CLICK
        homeButton?.onClick.AddListener( delegate {
            ScreenTransition.OnTransitionDone += OnMainScreenTransitionDone;
            
            GoToMenuAction?.Invoke( SceneBuildIndex.MainMenu );
        });
    }

    void OnMainScreenTransitionDone() {
        Time.timeScale = 1;

        ScreenTransition.OnTransitionDone -= OnMainScreenTransitionDone;
    }

    void SwitchPanelStates() => TogglePanelActive( toggleStatus: !isPanelActive );

    void TogglePanelActive( bool toggleStatus ) {
        isPanelActive = toggleStatus;

        Time.timeScale = isPanelActive ? 0 : 1;

        for( int index = 0; index < transform.childCount; index++ )
            transform.GetChild(index).gameObject.SetActive( toggleStatus );

        // TOGGLE INPUT ACTIONS
        // SET PAUSE BUTTONS ICONS
        // TOGGLE MUSIC AND SOUNDS
        // SET SCREEN TRANSITION COROUTINE
        OnGamePauseToggledAction?.Invoke( isPanelActive );
    }

    void OnDestroy() {
        PauseButton.PauseGameToggleAction -= SwitchPanelStates;
    }
}
