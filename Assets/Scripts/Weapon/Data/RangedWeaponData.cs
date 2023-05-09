using UnityEngine;


[CreateAssetMenu(fileName = "RangedWeaponData", menuName = "Weapon/RangedWeaponData")]
public class RangedWeaponData : WeaponData
{

    [Header("Shooting")]
    public int MaxDistance;

    [Header("Bullet")]
    public GameObject Bullet;
    public int BulletMoveSpeed;
    public Vector3 BulletSpawnPosition;
}
