using UnityEngine;

public class WeaponPickable : Interactable
{
    public BoxCollider2D Collider { get; private set; }
    private IWeapon weapon;
    private Player player;

    void Awake() {
        player = FindObjectOfType<Player>();
        weapon = transform.GetComponentInParent<IWeapon>();

        Collider = GetComponent<BoxCollider2D>();
    }

    // INITIALIZE
    public void Init() {
        // SET COLLIDER SIZE
        Collider.size = new Vector3( 
            weapon.Sprite.bounds.size.x / transform.lossyScale.x + 0.5f,
            weapon.Sprite.bounds.size.y / transform.lossyScale.y + 0.5f 
        );
    }

    public override void Interact() {
        player.Fight.OnWeaponPickup( floorWeapon: weapon );
    }
}
