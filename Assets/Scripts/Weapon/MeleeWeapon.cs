using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour, IWeapon
{
    private Collider2D weaponCollider;
    private Animator animator;

    void Start() {
        weaponCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void Attack( bool IsAttacking ) {
        animator.SetBool("IsAttacking", true);
        OnAttackWindupStarted();
    }
    
    void OnAttackWindupStarted() {
        weaponCollider.enabled = false;
    }
    
    void OnAttackWindupFinished() {
        weaponCollider.enabled = true;
    }

    public void OnAttackFinished() {
        weaponCollider.enabled = false;
        animator.SetBool("IsAttacking", false);
        ClearCollidedObjectIDList();
    }


    List<float> collidedObjectIDList = new();

    // AFTER BEING IN COLLISION
    void OnTriggerStay2D(Collider2D enemy) {
        if(animator.GetBool("IsAttacking")) {
            if( collidedObjectIDList.Contains( enemy.gameObject.GetInstanceID() ) )
                return;
            else
                collidedObjectIDList.Add( enemy.gameObject.GetInstanceID() );
            // DAMAGE ENEMY HERE
        }
    }

    public void ClearCollidedObjectIDList() {
        collidedObjectIDList = new();
    }
}
