using UnityEngine;
using System;

public class WeaponInteract : Interactable
{
    private BoxCollider2D cd;
    public IWeapon Weapon { get; private set; }

    
    // ACTIONS
    public static event Action<IWeapon> WeaponInteractAction;


    void Awake() {
        Weapon = transform.GetComponentInParent<IWeapon>();

        cd = GetComponent<BoxCollider2D>();
    }

    // INITIALIZE
    public void Init() {
        // SET COLLIDER SIZE
        cd.size = new Vector3( 
            Weapon.Sprite.bounds.size.x / transform.lossyScale.x + 0.5f,
            Weapon.Sprite.bounds.size.y / transform.lossyScale.y + 0.5f 
        );
    }

    public override void Interact() {
        WeaponInteractAction?.Invoke( Weapon );
    }
}
