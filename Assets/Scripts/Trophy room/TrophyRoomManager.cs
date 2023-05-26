using System.Threading.Tasks;
using System.Collections.Generic;

public class TrophyRoomManager : ToLoadingScreen
{
    private InputManager inputManager;
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;
    private List<Trophy> trophiesList;
    
    void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();
        trophiesList = new List<Trophy>();
        foreach( Trophy trophy in FindObjectsOfType<Trophy>() )
            trophiesList.Add( trophy );

        StartGamePortal.StartGameAction += StartTransitionToLoading;
    }

    async void Start() {
        screenTransition.Show();

        await Task.Delay( millisecondsDelay: 2000 );

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

    void OnDestroy() {
        StartGamePortal.StartGameAction -= StartTransitionToLoading;
    }
}
