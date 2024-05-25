
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;
using System;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateBefore(typeof(EntityDestroy))]

public partial class AssignTargetSystem : SystemBase
{
    private List<Entity> blueTeam = new List<Entity>();
    private List<Entity> redTeam = new List<Entity>();
    public Action<float3, Entity> OnSpawn;
    public bool StartButtel = true;
    EntityManager entityManager;

    [BurstCompile]
    protected override void OnUpdate()
    {

        if (UI.Instance.startGame)
        {
            // add each teams to list ------
            if (StartButtel)
            {
                entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                blueTeam = new List<Entity>();
                redTeam = new List<Entity>();

                Entities
                .WithAll<RedTeam>()
                .ForEach(
                (Entity entity, ref LocalTransform localTransform) =>
                {
                    redTeam.Add(entity);
                    OnSpawn?.Invoke(localTransform.Position, entity);

                }
                )
                .WithoutBurst()
                .Run();


                Entities
                .WithAll<BlueTeam>()
                .ForEach(
                    (Entity entity, ref LocalTransform localTransform) =>
                    {
                        blueTeam.Add(entity);
                        OnSpawn?.Invoke(localTransform.Position, entity);
                    }
                )
                .WithoutBurst()
                .Run();

                StartButtel = false;
            }
            //------------

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            // destroy unit if has EntityDestroy
            Entities
            .WithAll<EntityDestroy>()
            .WithAll<BlueTeam>()
            .ForEach((Entity entity) =>
               {
                   blueTeam.Remove(entity);
                   ecb.DestroyEntity(entity);
               }
            )
            .WithoutBurst()
            .Run();

            Entities
           .WithAll<EntityDestroy>()
           .WithAll<RedTeam>()
           .ForEach((Entity entity) =>
               {
                   redTeam.Remove(entity);
                   ecb.DestroyEntity(entity);
               }
           )
           .WithoutBurst()
           .Run();

            //--------------


            //check if unit found targert foreach teams -----

            Entities
            .WithAll<RedTeam>()
            .ForEach((Entity entity, ref LocalTransform localTransform, ref Target Target) =>
            {
                if (blueTeam.Count > 0 && Target.targetEntity == Entity.Null && !entityManager.HasComponent<EntityDestroy>(Target.targetEntity))
                {
                    var random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, 100000));
                    int rand = random.NextInt(0, blueTeam.Count);
                    if (!entityManager.HasComponent<EntityDestroy>(blueTeam[rand]))
                    {
                        Target.targetEntity = blueTeam[rand];
                        ecb.SetComponent(entity, new Target { targetEntity = blueTeam[rand] });
                    }
                }
                if (blueTeam.Count <= 0)
                {
                    UI.Instance.TeamWinText.text = $"<color=Red>{"Red Team Win"}</color>";
                    UI.Instance.EndScreen();

                }
            }
            )
            .WithoutBurst()
            .Run();


            Entities
            .WithAll<BlueTeam>()
            .ForEach((Entity entity, ref LocalTransform localTransform, ref Target target) =>
                {

                    if (!entityManager.HasComponent<EntityDestroy>(target.targetEntity) && redTeam.Count > 0 && target.targetEntity == Entity.Null)
                    {
                        var random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, 100000));
                        int rand = random.NextInt(0, redTeam.Count);
                        if (!entityManager.HasComponent<EntityDestroy>(redTeam[rand]))
                        {
                            target.targetEntity = redTeam[rand];
                            ecb.SetComponent(entity, new Target { targetEntity = redTeam[rand] });
                        }
                    }
                    if (redTeam.Count <= 0)
                    {
                        UI.Instance.TeamWinText.text = $"<color=blue>{"Blue Team Win"}</color>";
                        UI.Instance.EndScreen();
                    }

                }
            )
            .WithoutBurst()
            .Run();

            //----------

            ecb.Playback(entityManager);



        }

    }

}

