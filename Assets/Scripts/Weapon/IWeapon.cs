using UnityEngine;

public interface IWeapon
{
    public WeaponInteract WeaponInteract { get; }
    public WeaponData WeaponData { get; }
    public Transform Transform { get; }
    public GameObject GameObject { get; }
    public SpriteRenderer Sprite { get; }
    public Animator Animator { get; }
    public Timer RecoverTimer { get; }

    public bool IsRecovering { get; }
    
    public void Attack( bool IsAttacking );
    public void OnAttackWindupStarted();
    public void OnAttackWindupFinished();
    public void OnAttackFinished();
    public void OnRecoverFinished();
    
    // ON THIS WEAPON SWITCHED IN
    public void SwitchIn() {
        // ENABLE ANIMATIONS
        Animator.enabled = true;

        // SET POSITION ACCORDING TO WEAPON DATA
        Transform.localPosition = WeaponData.SwitchedInPosition;
        Transform.localRotation = Quaternion.Euler(WeaponData.SwitchedInRotation);

        // PLAY ANIMATION
        // animator.SetBool("IsInitializing", true);
    }

    // ON THIS WEAPON SWITCHED OUT
    public void SwitchOut() {
        // ENABLE ANIMATIONS
        Animator.enabled = false;

        // POSITION IS SET IN PARENT
       
        // PLAY ANIMATION
        // animator.SetBool("IsInitializing", true);
    }

    // SET THIS WEAPON'S SWITCHED OUT TRANSFORM
    public void SetSwitchedOutTransform( int lookState ) {
        Transform.localPosition = WeaponData.SwitchedOutPositions[lookState];
        Transform.localRotation = Quaternion.Euler(WeaponData.SwitchedOutRotations[lookState]);
    }

    public void PickUp() {
        // DISABLE WEAPON INTERACTABLE
        WeaponInteract.gameObject.SetActive(false);

        // ENABLE ANIMATIONS
        Animator.enabled = true;

        // SET POSITION ACCORDING TO WEAPON DATA
        Transform.localPosition = WeaponData.SwitchedInPosition;
        Transform.localRotation = Quaternion.Euler(WeaponData.SwitchedInRotation);

        // Z-INDEX
        Sprite.sortingLayerName = Constants.SORTING_LAYER_CHARACTER;

        // PLAY ANIMATION
        // animator.SetBool("IsInitializing", true);
    }

    public void Drop( Vector3 floorWeaponPosition, Quaternion floorWeaponRotation ) {
        // ENABLE WEAPON PICKABLE
        WeaponInteract.gameObject.SetActive(true);

        // DISABLE ANIMATIONS
        Animator.enabled = false;

        // SET POSITION ACCORDING TO WEAPON DATA
        Transform.localPosition = floorWeaponPosition;
        Transform.localRotation = Quaternion.Euler(new Vector3( 
            floorWeaponRotation.eulerAngles.x, 
            floorWeaponRotation.eulerAngles.y, 
            Random.Range(-180, 180) 
        ));

        // Z-INDEX
        Sprite.sortingLayerName = Constants.SORTING_LAYER_FLOOR_OBJECT;
    }
}
