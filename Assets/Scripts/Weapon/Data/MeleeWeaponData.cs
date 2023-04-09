using UnityEngine;


[CreateAssetMenu(fileName = "MeleeWeaponData", menuName = "Weapon/MeleeWeaponData")]
public class MeleeWeaponData : WeaponData
{
    [Header("Melee")]
    public int Damage;
    public int Knockback;
}
