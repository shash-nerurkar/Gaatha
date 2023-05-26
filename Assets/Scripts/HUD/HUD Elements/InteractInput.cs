using UnityEngine;
using UnityEngine.UI;
using System;

public class InteractInput : MonoBehaviour, HUDElement
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
    public void ToggleInteractable( bool isInteractable ) => button.interactable = isInteractable;

    // COMPONENTS
    private Button button;

    // EVENTS
    public static event Action InteractPressedAction;

    void Awake() {
        PlayerUI.InteractInputToggleAction += ToggleInteractable;

        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(delegate { 
            InteractPressedAction();
        });
    }

    void OnDestroy() {
        PlayerUI.InteractInputToggleAction -= ToggleInteractable;
    }
}
