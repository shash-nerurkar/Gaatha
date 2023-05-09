using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFight : MonoBehaviour
{
    // EVENTS
    public static event Action<Sprite, int> WeaponSpriteUpdateAction;

    void Awake() {
        FloorWeaponContainer = GameObject.FindGameObjectWithTag(Constants.FLOOR_WEAPON_CONTAINER_TAG);

        player = GetComponent<Player>();
        WeaponPivot = GetComponentInChildren<WeaponPivot>();
        WeaponQuiver = GetComponentInChildren<WeaponQuiver>();

        IsAttacking = false;

        // CREATING WEAPONS-IN-ARM LIST
        WeaponsInArm = new List<IWeapon>();
        // print("WeaponsInArm: " + WeaponsInArm.Count);
        foreach(IWeapon weapon in gameObject.GetComponentsInChildren<IWeapon>()) {
            WeaponsInArm.Add(weapon);
        }
        // print("WeaponsInArm: " + WeaponsInArm.Count);
        // foreach(IWeapon weapon in WeaponsInArm) {
        //     print(weapon.WeaponData.name);
        // }
        CurrentWeaponIndex = 0;

        WeaponSwitchInput.WeaponSwitchAction += OnWeaponSwitch;

        WeaponInteract.WeaponInteractAction += OnWeaponPickup;

        WeaponCraftingPanel.FetchWeaponInArmAction = SendWeaponsInArm;

        InputManager.OnPlayerWeaponAttackAction += OnWeaponAttack;
    }

    public void Init() {
        // CHOOSE 0TH WEAPON
        ChooseCurrentWeapon();

        // UPDATE HUD FOR WEAPONS
        for(int i = 0; i < WeaponsInArm.Count; i++) {
            WeaponSpriteUpdateAction?.Invoke( WeaponsInArm[i].Sprite.sprite, i );
        }
    }

    // DAMAGE RELATED STUFF
    [Header("Health")]
    private Player player;
    // int health = 100;

    public void TakeDamage( int damage ) {}


    
    // WEAPONS RELATED STUFF [Header("Weapons")]
    public GameObject FloorWeaponContainer { get; private set; }
    public WeaponPivot WeaponPivot { get; private set; }
    public WeaponQuiver WeaponQuiver { get; private set; }
    public List<IWeapon> WeaponsInArm { get; private set;}
    public int CurrentWeaponIndex { get; private set; }
    public bool IsAttacking { get; private set; }

    // FROM InputManager.cs
    public void OnWeaponAttack( bool IsAttacking ) {
        this.IsAttacking = IsAttacking;
    }
    
    void Update() {
        WeaponsInArm[CurrentWeaponIndex].Attack(IsAttacking: IsAttacking);
    }

    // FROM InputManager.cs
    public void OnWeaponSwitch() {
        // SWITCHING
        CurrentWeaponIndex = CurrentWeaponIndex == WeaponsInArm.Count - 1 ? 0 : CurrentWeaponIndex + 1;

        // SET OBJECTS
        ChooseCurrentWeapon();
    }

    private void ChooseCurrentWeapon() {
        // SWITCH WEAPONS IN WEAPON OBJECTS AND CHANGE THEIR PARENTS
        for ( int i = 0; i < WeaponsInArm.Count; i++ ) {
            if (i == CurrentWeaponIndex) {
                WeaponsInArm[i].Transform.SetParent( WeaponPivot.transform );
                WeaponsInArm[i].SwitchIn();
            }
            else {
                WeaponsInArm[i].Transform.SetParent( WeaponQuiver.transform );
                WeaponsInArm[i].SwitchOut();
            }
        }

        // SETTING Z-INDEX
        player.Look.SetWeaponsZIndex();
        // ROTATING SWITCHED-OUT WEAPONS
        player.Look.RotateSwitchedOutWeapons();

        // UPDATE HUD
        WeaponSpriteUpdateAction?.Invoke( WeaponsInArm[CurrentWeaponIndex].Sprite.sprite, CurrentWeaponIndex );
    }

    public void OnWeaponPickup( IWeapon floorWeapon ) {
        // DROP AND UPDATE WEAPON IN HAND
        WeaponsInArm[CurrentWeaponIndex].Transform.SetParent( FloorWeaponContainer?.transform );
        WeaponsInArm[CurrentWeaponIndex].Drop( floorWeaponTransform: floorWeapon.Transform );

        // PICK UP AND UPDATE FLOOR WEAPON 
        floorWeapon.Transform.SetParent( WeaponPivot.transform );
        floorWeapon.PickUp();

        // UPDATE WEAPONS-IN-ARM LIST
        WeaponsInArm[CurrentWeaponIndex] = floorWeapon;

        // // SETTING Z-INDEX
        player.Look.SetWeaponsZIndex();
        
        // UPDATE HUD
        WeaponSpriteUpdateAction?.Invoke( WeaponsInArm[CurrentWeaponIndex].Sprite.sprite, CurrentWeaponIndex );
    }

    void SendWeaponsInArm(out List<IWeapon> weaponsInArm) => weaponsInArm = WeaponsInArm;
}
