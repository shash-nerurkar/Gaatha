using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    // RANGED-SPECIFIC STUFF
    [SerializeField] private RangedWeaponData weaponData;
    public WeaponData WeaponData {
        get { return weaponData; }
    }
    private GameObject bulletContainer;

    // COMPONENTS
    private WeaponInteract weaponInteract;
    public WeaponInteract WeaponInteract {
        get { return weaponInteract; }
    }
    public Transform Transform {
        get { return transform; }
    }
    public GameObject GameObject {
        get { return gameObject; }
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
    private bool hasAmmo = true;
    private int currentBurstFireCount;
    private bool isRecovering = false;
    public bool IsRecovering {
        get { return isRecovering; }
    }

    void Awake() {
        bulletContainer = GameObject.FindGameObjectWithTag(tag: Constants.BULLET_CONTAINER_TAG);

        recoverTimer = gameObject.AddComponent<Timer>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        weaponInteract = GetComponentInChildren<WeaponInteract>();
    }

    void Start() {
        currentBurstFireCount = weaponData.BurstFireCount;
        
        weaponInteract.Init();
    }

    // IWeapon METHOD
    // START ATTACKING ANIMATION, IF NOT RELOADING
    public void Attack( bool isAttacking ) {
        if( isRecovering || animator.GetBool("isAttacking") ) return;

        switch( weaponData.AttackType ) {
            case WeaponAttackType.SemiAutomatic:
                if( isAttacking ) {
                    if(!hasAmmo) return;
                    hasAmmo = false;
                }
                else {
                    hasAmmo = true;
                    return;
                }
                break;
                
            case WeaponAttackType.BurstFire:
                if( isAttacking ) {
                    if(currentBurstFireCount <= 0) return;
                    currentBurstFireCount--;
                }
                else {
                    currentBurstFireCount = weaponData.BurstFireCount;
                    return;
                }
                break;
                
            case WeaponAttackType.FullyAutomatic:
                if( isAttacking ) {}
                else {
                    return;
                }
                break;
        }

        animator.SetBool("isAttacking", isAttacking);
    }
   
    // IWeapon METHOD
    public void OnAttackWindupStarted() {
        GameCamera.instance.Shake.ShakeCamera( intensity: 1, time: 0.3f );
        SoundManager.instance.Play(Constants.WAX_CROSSBOW_SOUND);
    }
    
    // IWeapon METHOD
    // ATTACK (SHOOT BULLET)
    public void OnAttackWindupFinished() {
        GameObject bulletInstance =  Instantiate(original: weaponData.Bullet, position: transform.TransformPoint(weaponData.BulletSpawnPosition), rotation: transform.rotation);
        
        bulletInstance.transform.SetParent(bulletContainer?.transform);
    }

    // IWeapon METHOD
    // START RELOADING, AND STOP ATTACK ANIMATION
    public void OnAttackFinished() {
        animator.SetBool("isAttacking", false);

        switch(weaponData.AttackType) {
            case WeaponAttackType.SemiAutomatic:
                isRecovering = true;
                recoverTimer.StartTimer(maxTime: weaponData.RecoveryTime, onTimerFinish: OnRecoverFinished);
                break;
                
            case WeaponAttackType.BurstFire:
                isRecovering = true;
                if(currentBurstFireCount > 0) {
                    animator.Play(stateName: Constants.IDLE_ANIMATION_STATE_NAME);
                    currentBurstFireCount--;
                    animator.SetBool("isAttacking", true);
                    return;
                }
                else {
                    recoverTimer.StartTimer(maxTime: weaponData.RecoveryTime, onTimerFinish: OnRecoverFinished);
                }
                break;
                
            case WeaponAttackType.FullyAutomatic:
                    animator.Play(stateName: Constants.IDLE_ANIMATION_STATE_NAME);
                return;
        }
    }

    // IWeapon METHOD
    public void OnRecoverFinished() {
        isRecovering = false;
        hasAmmo = true;
        currentBurstFireCount = WeaponData.BurstFireCount;
    }
}
