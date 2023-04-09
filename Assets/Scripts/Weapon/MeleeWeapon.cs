using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    // MELEE-SPECIFIC STUFF
    [SerializeField] private MeleeWeaponData weaponData;
    public WeaponData WeaponData {
        get { return weaponData; }
    }
    private Collider2D weaponCollider;
    
    // COMPONENTS
    private WeaponPickable weaponPickable;
    public WeaponPickable WeaponPickable {
        get { return weaponPickable; }
    }
    public Transform Transform {
        get { return transform; }
    }
    private Animator animator;
    public Animator Animator {
        get { return animator; }
    }
    private SpriteRenderer sprite;
    public SpriteRenderer Sprite {
        get { return sprite; }
    }
    private Timer recoverTimer;
    public Timer RecoverTimer {
        get { return recoverTimer; }
    }
    
    // VARIABLES
    private bool isRecovering = false;
    public bool IsRecovering {
        get { return isRecovering; }
    }

    void Start() {
        recoverTimer = gameObject.AddComponent<Timer>();
        sprite = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        weaponPickable = GetComponentInChildren<WeaponPickable>();
        weaponPickable.Init();
    }

    // IWeapon METHOD
    // START ATTACKING ANIMATION, IF NOT RELOADING
    public void Attack( bool IsAttacking ) {
        if(!isRecovering) animator.SetBool("isAttacking", IsAttacking);
    }

    // IWeapon METHOD
    // DISABLE THE COLLIDER, SINCE HERE THE USER IS NOT ATTACKING
    public void OnAttackWindupStarted() {
        weaponCollider.enabled = false;
    }
    
    // IWeapon METHOD
    // ATTACK
    public void OnAttackWindupFinished() {
        weaponCollider.enabled = true;
    }

    // IWeapon METHOD
    // START RELOADING, DISABLE THE COLLIDER, CLEAR THE COLLIDED OBJECT LIST, AND STOP ATTACK ANIMATION
    public void OnAttackFinished() {
        weaponCollider.enabled = false;
        isRecovering = true;
        recoverTimer.StartTimer(maxTime: 1, onTimerFinish: OnRecoverFinished);
        animator.SetBool("isAttacking", false);
        ClearCollidedObjectIDList();
    }
   
    // IWeapon METHOD
    public void OnRecoverFinished() => isRecovering = false;

    List<float> collidedObjectIDList = new();

    // AFTER BEING IN COLLISION
    void OnTriggerStay2D( Collider2D enemy ) {
        // IF THE OBJECT IS IN THE LIST, SKIP THE OBJECT
        if( collidedObjectIDList.Contains( enemy.gameObject.GetInstanceID() ) )
            return;
        // ELSE, ADD THE OBJECT TO THE COLLIDED OBJECT LIST AND EXECUTE THE CODE
        else
            collidedObjectIDList.Add( enemy.gameObject.GetInstanceID() );
        // DAMAGE ENEMY HERE
    }

    // CLEAR THE COLLIDED OBJECT LIST
    public void ClearCollidedObjectIDList() {
        collidedObjectIDList = new();
    }
}
