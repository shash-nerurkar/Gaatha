using UnityEngine;

public class HUD : MonoBehaviour
{
    public Player Player { get; private set; }
    public WeaponSwitchInput weaponSwitchInput { get; private set; }
    public WeaponPickupInput weaponPickupInput { get; private set; }
    public WeaponCraftingInput weaponCraftingInput { get; private set; }

    void Awake() {
        weaponSwitchInput = GetComponentInChildren<WeaponSwitchInput>();
        weaponPickupInput = GetComponentInChildren<WeaponPickupInput>();
        weaponCraftingInput = GetComponentInChildren<WeaponCraftingInput>();
    }

    // FROM World.cs
    public void Init( Player player ) {
        Player = player;

        weaponSwitchInput.Init( HUD: GetComponent<HUD>() );
        weaponPickupInput.Init( HUD: GetComponent<HUD>() );
        weaponCraftingInput.Init( HUD: GetComponent<HUD>() );
    }
}
