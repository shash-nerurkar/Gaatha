using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    void Awake() {
        IsAttacking = false;
        // playerWeapons = new List<IFPSWeapon>();
        // foreach(IFPSWeapon weapon in gameObject.GetComponentsInChildren<IFPSWeapon>()) {
        //     playerWeapons.Add(weapon);
        // }
        // currentWeaponIndex = 1;
        // List<Sprite> weaponSprites = new();
        // foreach(IFPSWeapon weapon in playerWeapons) {
        //     player.UI.SetWeaponPanel(weaponSprite: weapon.Image, index: playerWeapons.IndexOf(weapon));
        // }
        // OnWeaponSwitch(switchDirection: -1);
    }

    [Header("Health")]
    private Player player;
    // int health = 100;

    public void TakeDamage( int damage ) {

    }


    [Header("Weapon")]
    private Camera cam;

    // public List<Weapon> playerWeapons { get; private set;}
    public int currentWeaponIndex { get; private set;}
    public bool IsAttacking { get; private set; }

    public void OnWeaponAttack(bool IsAttacking) {
        this.IsAttacking = IsAttacking;
    }
    void Update() {
        // playerWeapons[currentWeaponIndex].Attack(IsAttacking: IsAttacking);
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
