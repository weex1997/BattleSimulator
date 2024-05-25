using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MoveAndDamageSystem : SystemBase
{
    //for UI display
    public Action<float, float3, Entity> OnDealDamage;
    public Action<float3, Entity> OnMove;

    [BurstCompile]
    protected override void OnUpdate()
    {

        if (UI.Instance.startGame)
        {

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);


            //blue team -------------

            Entities
            .WithAll<BlueTeam>()
            .WithNone<EntityDestroy>()
            .ForEach((Entity entity, ref Target Target, ref LocalTransform localTransform, ref UnitProperties blueTeamUnitProperties, ref Health blueTeamHealthProperties) =>
                {

                    //movement function -------

                    if (Target.targetEntity != Entity.Null)
                    {
                        LocalTransform trgetTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(Target.targetEntity);
                        var dist = math.distance(trgetTransform.Position, localTransform.Position);

                        if (dist > blueTeamUnitProperties.AttackRange)
                        {
                            if (trgetTransform.Position.x > localTransform.Position.x)
                            {
                                localTransform.Position.x += blueTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                            }
                            else
                            {
                                localTransform.Position.x -= blueTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                            }

                            if (trgetTransform.Position.z > localTransform.Position.z)
                            {
                                localTransform.Position.z += blueTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                            }
                            else
                            {
                                localTransform.Position.z -= blueTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                            }

                            OnMove?.Invoke(localTransform.Position, entity);

                        }
                        else
                        {
                            //Attack function -----

                            var tagetHp = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Health>(Target.targetEntity);
                            var tagetPostion = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(Target.targetEntity);

                            if (tagetHp.HP > 0)
                            {
                                if (blueTeamUnitProperties.UpdatedAttackSpeed >= 0)
                                {
                                    blueTeamUnitProperties.UpdatedAttackSpeed -= SystemAPI.Time.DeltaTime;
                                }
                                else
                                {
                                    float hp = tagetHp.HP - blueTeamUnitProperties.AttackDamage;
                                    Debug.Log(Target.targetEntity + " have health: " + hp);
                                    ecb.SetComponent(Target.targetEntity, new Health { HP = hp });
                                    blueTeamUnitProperties.UpdatedAttackSpeed = blueTeamUnitProperties.InitialAttackSpeed;
                                    OnDealDamage?.Invoke(hp, tagetPostion.Position, Target.targetEntity);

                                }
                            }
                            else
                            {
                                //before add EntityDestroy tag find to the entity new target ----

                                if (Target.targetEntity != Entity.Null)
                                {
                                    var tt = Target.targetEntity;
                                    Target.targetEntity = Entity.Null;
                                    ecb.AddComponent<EntityDestroy>(tt);
                                }
                            }

                        }
                    }
                })
                .WithoutBurst()
                .Run();



            //red team ------------

            Entities
            .WithAll<RedTeam>()
            .WithNone<EntityDestroy>()
            .ForEach((Entity entity, ref Target Target, ref LocalTransform localTransform, ref UnitProperties redTeamUnitProperties, ref Health health) =>
               {
                   if (Target.targetEntity != Entity.Null)
                   {
                       LocalTransform trgetTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(Target.targetEntity);

                       var dist = math.distance(trgetTransform.Position, localTransform.Position); // distance
                       if (dist > redTeamUnitProperties.AttackRange) // moving every axis seperatly to the target , depending on the Attack range ,snipping it
                       {
                           if (trgetTransform.Position.x > localTransform.Position.x)
                           {
                               localTransform.Position.x += redTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                           }
                           else
                           {
                               localTransform.Position.x -= redTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                           }

                           if (trgetTransform.Position.z > localTransform.Position.z)
                           {
                               localTransform.Position.z += redTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                           }
                           else
                           {
                               localTransform.Position.z -= redTeamUnitProperties.MovementSpeed * SystemAPI.Time.DeltaTime;
                           }

                           OnMove?.Invoke(localTransform.Position, entity);

                       }
                       else
                       {
                           var tagetHp = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Health>(Target.targetEntity);
                           var tagetPostion = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(Target.targetEntity);


                           if (tagetHp.HP > 0)
                           {
                               if (redTeamUnitProperties.UpdatedAttackSpeed >= 0)
                               {
                                   redTeamUnitProperties.UpdatedAttackSpeed -= SystemAPI.Time.DeltaTime;
                               }
                               else
                               {
                                   float hp = tagetHp.HP - redTeamUnitProperties.AttackDamage;
                                   OnDealDamage?.Invoke(hp, tagetPostion.Position, Target.targetEntity);


                                   Debug.Log(Target.targetEntity + " have health: " + hp);
                                   ecb.SetComponent(Target.targetEntity, new Health { HP = hp });
                                   redTeamUnitProperties.UpdatedAttackSpeed = redTeamUnitProperties.InitialAttackSpeed;
                               }
                           }
                           else
                           {
                               if (Target.targetEntity != Entity.Null)
                               {
                                   var tt = Target.targetEntity;
                                   Target.targetEntity = Entity.Null;
                                   ecb.AddComponent<EntityDestroy>(tt);
                               }
                           }
                       }
                   }
               }
           )
           .WithoutBurst()
           .Run();

            ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
        }

    }

}
