using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // COMPONENTS
    private Player player;
    private Animator animator;

    // VARIABLES
    public Vector2 LookDirection { get; private set; }
    private readonly float[] equippedWeaponXRotationRange = { 40, 50 };
    private readonly float[] equippedWeaponYRotationRange = { 25, 0 };
    private readonly float[] equippedWeaponZRotationRange = {  0, 0 };
    void Awake() {
        LookDirection = new Vector2(1, 0);

        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        InputManager.OnPlayerLookAction += OnLook;
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
            animator.SetInteger(
                "lookState", 
                tempLookAngle < 22.5f ? 4 :
                tempLookAngle < 67.5f ? 3 :
                tempLookAngle < 112.5f ? 2 :
                tempLookAngle < 157.5f ? 1 : 0
            );

            // ROTATING SWITCHED-OUT WEAPONS
            RotateSwitchedOutWeapons();

            // ROTATING SWITCHED-IN WEAPON
            lookAngle = -lookAngle;
            // SETTING ROTATION-Z VALUE
            Vector3 weaponRot = new Vector3( equippedWeaponZRotationRange[0], equippedWeaponZRotationRange[1], lookAngle );
            
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
                weaponRot.y = Mathf.Lerp( -equippedWeaponYRotationRange[0], equippedWeaponYRotationRange[1], tValueY );
            }
            else {
                // lookAngle RANGE IS [0, 45] HERE
                tValueY = Mathf.InverseLerp( 0, 45, Mathf.Abs( lookAngle - 135 ) );
                // lookAngle RANGE IS [0, 90] HERE
                tValueX = Mathf.InverseLerp( 0, 90, lookAngle - 90 );

                // SETTING ROTATION-Y VALUE
                weaponRot.y = Mathf.Lerp( equippedWeaponYRotationRange[0], equippedWeaponYRotationRange[1], tValueY );
            }

            // SETTING ROTATION-X VALUE
            weaponRot.x = Mathf.Lerp( equippedWeaponXRotationRange[0], equippedWeaponXRotationRange[1], tValueX );

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
        // TOPMOST POINT IS 0, BOTTOM MOST IS 180 => lookAngle RANGE IS [-180, 180]
        float lookAngle = Mathf.Abs( -Mathf.Atan2( LookDirection.x, LookDirection.y ) * Mathf.Rad2Deg );
        bool showCurrentWeaponOnTop = lookAngle >= 90;

        if( showCurrentWeaponOnTop ) {
            player.Fight.WeaponPivot.SortingGroup.sortingOrder =   1;
            player.Fight.WeaponQuiver.SortingGroup.sortingOrder = -1;
        }
        else {
            player.Fight.WeaponPivot.SortingGroup.sortingOrder =  -1;
            player.Fight.WeaponQuiver.SortingGroup.sortingOrder =  1;
        }
    }
}
