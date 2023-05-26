using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor;

public class WorldManager : ToLoadingScreen
{
    private InputManager inputManager;
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;

    [SerializeField] private List<GameObject> allWeapons;
    
    // FUNCS
    public delegate void GetAllWeaponsInGameDelegate( out List<IWeapon> weaponsInArm );

    void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();

        WeaponCraftingResultFinder.GetAllGameWeaponsAction += GetAllGameWeapons;

        WeaponCraftingPanel.GetAllGameWeaponsAction += GetAllGameWeapons;
    }

    void GetAllGameWeapons( out List<IWeapon> weaponsInArm ) {
        weaponsInArm = new List<IWeapon>();

        foreach( GameObject weaponObject in allWeapons ) {
            IWeapon weapon = weaponObject.GetComponent<IWeapon>();
            if( weapon != null )
                weaponsInArm.Add( weapon );
        }
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

    void OnDestroy() {
        WeaponCraftingResultFinder.GetAllGameWeaponsAction -= GetAllGameWeapons;

        WeaponCraftingPanel.GetAllGameWeaponsAction -= GetAllGameWeapons;
    }
}
