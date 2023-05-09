using UnityEngine;

public class PauseInput : MonoBehaviour, HUDElement
{
    [SerializeField] private Vector3 showPosition;
    public Vector3 ShowPosition {
        get { return showPosition; }
        set { showPosition = value; }
    }
    
    [SerializeField] private Vector3 hidePosition;
    public Vector3 HidePosition {
        get { return hidePosition; }
        set { hidePosition = value; }
    }
    
    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }
    private RectTransform rectTransform;
    public RectTransform RectTransform {
        get { return rectTransform; }
        set { rectTransform = value; }
    }

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }


    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}
}
