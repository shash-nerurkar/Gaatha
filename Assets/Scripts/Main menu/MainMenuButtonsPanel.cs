using UnityEngine;
using System;

public class MainMenuButtonsPanel : MonoBehaviour, HUDElement
{
    [SerializeField] private Vector3 showPosition;
    public Vector3 ShowPosition {
        get { return showPosition; }
    }
    
    [SerializeField] private Vector3 hidePosition;
    public Vector3 HidePosition {
        get { return hidePosition; }
    }
    
    private RectTransform rectTransform;
    public RectTransform RectTransform {
        get { return rectTransform; }
    }
    
    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }

    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}
    
    // EVENTS
    public static event Action<SceneBuildIndex> StartGameAction;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnNewGamePressed() {
        StartGameAction?.Invoke( SceneBuildIndex.TrophyRoom );
    }

    public void OnContinueGamePressed() {
        StartGameAction?.Invoke( SceneBuildIndex.World );
    }

    public void OnOptionsPressed() {
        
    }

    public void OnCreditsPressed() {
        
    }

    public void OnQuitPressed() {
        Application.Quit();
    }
}
