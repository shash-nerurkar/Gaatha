using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // COMPONENTS
    private Player player;
    private Animator animator;

    // VARIABLES
    public Vector2 LookDirection { get; private set; }
    
    void Awake() {
        LookDirection = new Vector2(1, 0);
    }

    void Start() {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        // OnLook( new Vector2( 0, -1 ) );
    }

    // FROM InputManager.cs
    public void OnLook( Vector2 lookDirection ) {
        if(lookDirection != Vector2.zero) {
            LookDirection = lookDirection;

            // TOPMOST POINT IS 0, BOTTOM MOST IS 180 => lookAngle RANGE IS [-180, 180]
            float lookAngle = Mathf.Atan2( lookDirection.x, lookDirection.y ) * Mathf.Rad2Deg;

            // ROTATING PLAYER
            transform.localScale = new Vector3(Mathf.Sign(lookAngle), 1, 1);

            float tempLookAngle = Mathf.Abs(lookAngle);
            if(tempLookAngle < 22.5f) {
                animator.SetInteger("lookState", 4);
            }
            else if(tempLookAngle < 67.5f) {
                animator.SetInteger("lookState", 3);
            }
            else if(tempLookAngle < 112.5f) {
                animator.SetInteger("lookState", 2);
            }
            else if(tempLookAngle < 157.5f) {
                animator.SetInteger("lookState", 1);
            }
            else {
                animator.SetInteger("lookState", 0);
            }

            // ROTATING SWITCHED-OUT WEAPONS
            RotateSwitchedOutWeapons();

            // ROTATING SWITCHED-IN WEAPON
            lookAngle = -lookAngle;
            // SETTING ROTATION-Z VALUE
            Vector3 weaponRot = new Vector3( 0, 0, lookAngle );
            
            lookAngle = Mathf.Abs( lookAngle );
            float tValueX, tValueY;

            // SETTING WEAPONS Z-INDEX
            SetWeaponsZIndex();

            if( lookAngle < 90 ) {                
                // lookAngle RANGE IS [0, 45] NOW
                tValueY = Mathf.InverseLerp( 0, 45, Mathf.Abs( lookAngle - 45 ) );
                // lookAngle RANGE IS [0, 90] HERE
                tValueX = Mathf.InverseLerp( 0, 90, Mathf.Abs( lookAngle - 90 ) );

                // SETTING ROTATION-Y VALUE
                weaponRot.y = Mathf.Lerp( -25, 0, tValueY );
            }
            else {
                // lookAngle RANGE IS [0, 45] HERE
                tValueY = Mathf.InverseLerp( 0, 45, Mathf.Abs( lookAngle - 135 ) );
                // lookAngle RANGE IS [0, 90] HERE
                tValueX = Mathf.InverseLerp( 0, 90, lookAngle - 90 );

                // SETTING ROTATION-Y VALUE
                weaponRot.y = Mathf.Lerp( 25, 0, tValueY );
            }

            // SETTING ROTATION-X VALUE
            weaponRot.x = Mathf.Lerp( 40, 50, tValueX );

            player.Fight.WeaponPivot.transform.rotation = Quaternion.Euler( weaponRot );
        }
    }

    // SET THE POSITION AND ROTATION OF THE SWITCHED OUT WEAPON ACCORDING TO THE PLAYER LOOK DIRECTION
    public void RotateSwitchedOutWeapons() {
        if(player == null || player.Fight == null || player.Fight.WeaponsInArm == null) return;
        
        for(int i = 0; i < player.Fight.WeaponsInArm.Count; i++) {
            if(i != player.Fight.CurrentWeaponIndex) 
                player.Fight.WeaponsInArm[i].SetSwitchedOutTransform(lookState: animator.GetInteger("lookState"));
        }
    }
    
    // SET THE Z-INDEX VALUES OF EACH WEAPON ACCORDING TO THE PLAYER LOOK DIRECTION
    public void SetWeaponsZIndex() {
        if(player == null || player.Fight == null || player.Fight.WeaponsInArm == null) return;

        // TOPMOST POINT IS 0, BOTTOM MOST IS 180 => lookAngle RANGE IS [-180, 180]
        float lookAngle = Mathf.Abs( -Mathf.Atan2( LookDirection.x, LookDirection.y ) * Mathf.Rad2Deg );
        bool showCurrentWeaponOnTop = lookAngle >= 90;

        for(int i = 0; i < player.Fight.WeaponsInArm.Count; i++) {
            if(i == player.Fight.CurrentWeaponIndex) player.Fight.WeaponsInArm[i].Sprite.sortingOrder = showCurrentWeaponOnTop ? 1 : -1;
            else  player.Fight.WeaponsInArm[i].Sprite.sortingOrder = showCurrentWeaponOnTop ? -1 : 1;
        }
    }
}
