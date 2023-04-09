using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupInput : MonoBehaviour, HUDElement
{
    [SerializeField] private Image weapon1Image;
    [SerializeField] private Image weapon2Image;

    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }

    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) => button.interactable = isInteractable;

    // COMPONENTS
    private Button button;

    void Awake() {
        button = GetComponent<Button>();
    }

    void Start() {
        button.onClick.AddListener(delegate { 
            HUD.Player.Interact.PickUpWeapon();
        });
    }

    void SetWeaponSprite(Sprite sprite, int index) {
        switch(index) {
            case 1:
                weapon1Image.sprite = sprite;
                break;
            
            case 2:
                weapon2Image.sprite = sprite;
                break;
        }
    }
}
