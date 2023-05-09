using UnityEngine;
using System.Threading.Tasks;


public class WorldManager : MonoBehaviour
{
    private InputManager inputManager;
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;

    void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();
    }

    async void Start() {
        screenTransition.Show();

        await Task.Delay( millisecondsDelay: 1000 );

        screenTransition.FadeOut();

        HUD.AdjustHUDElements(
            showMovementInput: true,
            showWeaponShootInput: true,
            showWeaponSwitchInput: true,
            showInteractInput: true,
            showPauseInput: true,
            showWeaponCraftingInput: true,
            duration: 0.75f
        );

        player.Fight.Init();

        inputManager.EnableOnFootActions();
    }
}
