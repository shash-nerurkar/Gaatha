using UnityEngine;


[CreateAssetMenu(fileName = "RangedWeaponData", menuName = "Weapon/RangedWeaponData")]
public class RangedWeaponData : WeaponData
{

    [Header("Shooting")]
    public int MaxDistance;

    [Header("Bullet")]
    public GameObject Bullet;
    public int BulletDamage;
    public int BulletMoveSpeed;
    public int BulletKnockback;
    public Vector3 BulletSpawnPosition;
}
