using Unity.Entities;
using UnityEngine;

public class UnitConfigAuthoring : MonoBehaviour
{
    [SerializeField]
    private UnitConfiguration unitConfiguration;

    private class UnitConfigBaker : Baker<UnitConfigAuthoring>
    {
        public override void Bake(UnitConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var playerConfiguration = new UnitProperties();
            var playerConfigurationHP = new Health();


            // we will use the method that we already create before to parsing data into the component
            authoring.unitConfiguration.ConvertToUnManaged(ref playerConfiguration, ref playerConfigurationHP);


            // Add configuration to entity

            UnitProperties unitProperties = default;
            Health helth = default;

            helth.HP = authoring.unitConfiguration.HP;

            unitProperties.AttackDamage = authoring.unitConfiguration.AttackDamage;
            unitProperties.InitialAttackSpeed = authoring.unitConfiguration.InitialAttackSpeed;
            unitProperties.AttackRange = authoring.unitConfiguration.AttackRange;
            unitProperties.MovementSpeed = authoring.unitConfiguration.MovementSpeed;
            AddComponent(entity, unitProperties);
            AddComponent(entity, helth);

            DependsOn(authoring.unitConfiguration);

        }
    }
}