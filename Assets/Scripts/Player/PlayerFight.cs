using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    void Awake() {
        
    }

    void Update() {
        
    }

    [Header("Health")]
    [SerializeField] private Player player;
    // int health = 100;

    public void TakeDamage(int damage) {

    }

    [Header("Weapon")]
    [SerializeField] private Camera cam;

    // public List<Weapon> playerWeapons { get; private set;}
    public int currentWeaponIndex { get; private set;}
    [HideInInspector] public bool IsAttacking { get; set; }

    public void OnWeaponAttack(bool IsAttacking) {
        this.IsAttacking = IsAttacking;
    }

    public void OnWeaponReload() {
        // if(playerWeapons[currentWeaponIndex] is FPSGun) 
        //     (playerWeapons[currentWeaponIndex] as FPSGun).Reload();
    }

    public void OnWeaponSwitch(float switchDirection) {
        // if(switchDirection == -1)
        //     currentWeaponIndex = currentWeaponIndex == 0 ? playerWeapons.Count - 1 : currentWeaponIndex - 1;
        // else 
        //     currentWeaponIndex = currentWeaponIndex == playerWeapons.Count - 1 ? 0 : currentWeaponIndex + 1;
        // ChooseCurrentWeapon();
    }

    public void OnWeaponPickup() {//IFPSWeapon floorWeapon) {
        // if(playerWeapons[currentWeaponIndex] is FPSGun) 
        //     if(!(playerWeapons[currentWeaponIndex] as FPSGun).CanDrop()) 
        //         return;
        
        // playerWeapons[currentWeaponIndex].Drop(floorWeaponTransform: floorWeapon.WeaponTransform);
        // floorWeapon.PickUp();
        // player.UI.SetWeaponPanel(weaponSprite: floorWeapon.Image, index: currentWeaponIndex);
        // playerWeapons[currentWeaponIndex] = floorWeapon;
    }

    private void ChooseCurrentWeapon() {
        // for(int i = 0; i < playerWeapons.Count; i++) {
        //     if(i == currentWeaponIndex) playerWeapons[i].SwitchIn();
        //     else playerWeapons[i].SwitchOut();
        // }

        // player.UI.SwitchWeapon(currentWeaponIndex: currentWeaponIndex);
    }
}
