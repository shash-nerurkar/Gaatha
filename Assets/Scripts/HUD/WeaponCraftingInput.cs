using UnityEngine;
using UnityEngine.UI;

public class WeaponCraftingInput : MonoBehaviour, HUDElement
{
    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }

    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}

    // COMPONENTS
    private Button button;

    void Awake() {
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(delegate { 
            // START CRAFTING HERE
        });
    }
}
