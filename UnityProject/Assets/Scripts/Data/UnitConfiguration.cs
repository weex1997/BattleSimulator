using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamUnitsData", menuName = "Scriptable Object/Team Units Data", order = 0)]

public class UnitConfiguration : ScriptableObject
{

    public float HP;
    public float AttackDamage;
    public float InitialAttackSpeed;
    public float AttackRange;
    public float MovementSpeed;

    // This is a util method use for baking process.
    public void ConvertToUnManaged(ref UnitProperties data, ref Health hp)
    {
        hp.HP = HP;
        data.AttackDamage = AttackDamage;
        data.InitialAttackSpeed = InitialAttackSpeed;
        data.AttackRange = AttackRange;
        data.MovementSpeed = MovementSpeed;
    }
}
