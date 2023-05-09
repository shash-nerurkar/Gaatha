using UnityEngine;
using UnityEngine.UI;

public class WeaponCraftingInput : MonoBehaviour, HUDElement
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


    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}

    // COMPONENTS
    private Button button;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(delegate { 
            // START CRAFTING HERE
        });
    }
}
