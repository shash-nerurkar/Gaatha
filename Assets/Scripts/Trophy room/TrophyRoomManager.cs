using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class TrophyRoomManager : ToLoadingScreen
{
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;
    private List<Trophy> trophiesList;
    
    // ACTIONS
    public static event Action EnablePlayerOnFootActionsAction;
    public static event Action DisableAllActionsAction;
    
    void Awake() {
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();
        trophiesList = new List<Trophy>();
        foreach( Trophy trophy in FindObjectsOfType<Trophy>() )
            trophiesList.Add( trophy );

        StartGamePortal.StartGameAction += StartTransitionToLoading;

        PausePanel.GoToMenuAction += StartTransitionToLoading;
        PausePanel.OnGamePauseToggledAction += OnGamePauseToggled;
    }

    async void Start() {
        screenTransition.Show();

        await Task.Delay( millisecondsDelay: 1000 );

        SoundManager.instance.Play(Constants.HAPPY_DAYS_MUSIC);

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

        EnablePlayerOnFootActionsAction?.Invoke();
    }

    void OnGamePauseToggled( bool toggleStatus ) {
        if( toggleStatus ) {
            DisableAllActionsAction?.Invoke();
        }
        else {
            EnablePlayerOnFootActionsAction?.Invoke();
        }
    }

    void OnDestroy() {
        StartGamePortal.StartGameAction -= StartTransitionToLoading;

        PausePanel.GoToMenuAction -= StartTransitionToLoading;
        PausePanel.OnGamePauseToggledAction -= OnGamePauseToggled;
    }
}
