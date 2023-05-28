using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class WorldManager : ToLoadingScreen
{
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;

    [SerializeField] private List<GameObject> allWeapons;
    
    // FUNCS
    public delegate void GetAllWeaponsInGameDelegate( out List<IWeapon> weaponsInArm );
    
    // ACTIONS
    public static event Action EnablePlayerOnFootActionsAction;
    public static event Action DisableAllActionsAction;

    void Awake() {
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();

        WeaponCraftingResultFinder.GetAllGameWeaponsAction += GetAllGameWeapons;

        WeaponCraftingPanel.GetAllGameWeaponsAction += GetAllGameWeapons;

        PausePanel.GoToMenuAction += StartTransitionToLoading;
        PausePanel.OnGamePauseToggledAction += OnGamePauseToggled;
    }

    async void Start() {
        screenTransition.Show();

        await Task.Delay( millisecondsDelay: 1000 );

        SoundManager.instance.Play(Constants.THE_CHASE_MUSIC);

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

    void GetAllGameWeapons( out List<IWeapon> weaponsInArm ) {
        weaponsInArm = new List<IWeapon>();

        foreach( GameObject weaponObject in allWeapons ) {
            IWeapon weapon = weaponObject.GetComponent<IWeapon>();
            if( weapon != null )
                weaponsInArm.Add( weapon );
        }
    }

    void OnDestroy() {
        WeaponCraftingResultFinder.GetAllGameWeaponsAction -= GetAllGameWeapons;

        WeaponCraftingPanel.GetAllGameWeaponsAction -= GetAllGameWeapons;

        PausePanel.GoToMenuAction -= StartTransitionToLoading;
        PausePanel.OnGamePauseToggledAction -= OnGamePauseToggled;
    }
}
