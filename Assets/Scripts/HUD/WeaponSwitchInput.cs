using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchInput : MonoBehaviour, HUDElement
{
    private Animator animator;
    [SerializeField] private Image weapon1Image;
    [SerializeField] private Image weapon2Image;

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
        animator = GetComponent<Animator>();

        button.onClick.AddListener(delegate {
            animator.SetInteger( "weaponNo", animator.GetInteger("weaponNo") == 2 ? 1 : 2 );

            // PLAYER SWITCH WEAPON
            HUD.Player.Fight.OnWeaponSwitch();
        });
    }
}
