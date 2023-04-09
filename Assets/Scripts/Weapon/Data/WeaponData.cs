using System.Collections.Generic;
using UnityEngine;


public abstract class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Attacking")]
    public WeaponAttackType AttackType;
    public int RecoveryTime;

    [Header("Switching")]
    public Vector3 SwitchedInPosition;
    public Vector3 SwitchedInRotation;
    public List<Vector3> SwitchedOutPositions;
    public List<Vector3> SwitchedOutRotations;

    [Header("Gun Shoot Type: Burst Fire")]
    public int BurstFireCount;
}
