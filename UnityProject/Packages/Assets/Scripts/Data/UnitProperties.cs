using Unity.Entities;

public struct UnitProperties : IComponentData
{
    public float AttackDamage;
    public float InitialAttackSpeed;
    public float UpdatedAttackSpeed;

    public float AttackRange;
    public float MovementSpeed;

}

public struct Health : IComponentData
{
    public float HP;

}

