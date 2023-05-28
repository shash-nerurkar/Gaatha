using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponCraftingInput : MonoBehaviour, HUDElement
{
    [SerializeField] private Vector3 showPosition;
    public Vector3 ShowPosition {
        get { return showPosition; }
    }
    
    [SerializeField] private Vector3 hidePosition;
    public Vector3 HidePosition {
        get { return hidePosition; }
    }
    
    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }
    
    private RectTransform rectTransform;
    public RectTransform RectTransform {
        get { return rectTransform; }
    }


    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}

    // COMPONENTS
    private Button button;

    // ACTIONS
    public static event Action StartWeaponCraftingAction;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(delegate {
            StartWeaponCraftingAction?.Invoke();
        });
    }
}
