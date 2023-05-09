using UnityEngine;
using System;

public class WeaponInteract : Interactable
{
    private BoxCollider2D cd;
    private IWeapon weapon;

    
    // EVENTS
    public static event Action<IWeapon> WeaponInteractAction;


    void Awake() {
        weapon = transform.GetComponentInParent<IWeapon>();

        cd = GetComponent<BoxCollider2D>();
    }

    // INITIALIZE
    public void Init() {
        // SET COLLIDER SIZE
        cd.size = new Vector3( 
            weapon.Sprite.bounds.size.x / transform.lossyScale.x + 0.5f,
            weapon.Sprite.bounds.size.y / transform.lossyScale.y + 0.5f 
        );
    }

    public override void Interact() {
        WeaponInteractAction?.Invoke( weapon );
    }
}
