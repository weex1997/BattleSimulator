using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class SpawnSystem : SystemBase
{
    private EntityManager entityManager;
    private Entity unitEntity;
    private Entity EntityCorrdinates;
    public bool isChange = false;
    public int lineupIndex = 0;
    public List<Entity> Lineups = new List<Entity>();

    [BurstCompile]
    protected override void OnUpdate()
    {

        if (LevelManager.Instance.StartTheLevel)
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Lineups = new List<Entity>();
            lineupIndex = 0;

            Entities
            .WithAll<GameProperties>()
            .ForEach(
            (Entity entity) =>
            {
                unitEntity = entity;
            }
            )
            .WithoutBurst()
            .Run();

            var unit = SystemAPI.GetAspect<GameDataAspect>(unitEntity);

            //add lineups to the list
            Entities
            .WithAll<Lineup>()
            .ForEach(
            (Entity entity) =>
            {
                Lineups.Add(entity);
            }
            )
            .WithoutBurst()
            .Run();

            isChange = true;

            //spawn red team on game start
            SpawnRedTeam();
            LevelManager.Instance.StartTheLevel = false;
        }

        if (isChange)
        {
            EntityCorrdinates = Lineups[lineupIndex];
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var unit = SystemAPI.GetAspect<GameDataAspect>(unitEntity);

            //when choose another lineup previous units will destroy
            Entities
            .WithAll<DestroyTag>()
            .ForEach(
            (Entity entity) =>
            {
                ecb.DestroyEntity(entity);

            }
            )
            .WithoutBurst()
            .Run();
            ecb.Playback(entityManager);

            //spawn blue team
            SpawnBlueTeam();

            isChange = false;
        }
    }
    public void SpawnRedTeam()
    {

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var unit = SystemAPI.GetAspect<GameDataAspect>(unitEntity);

        //random lineup for red team
        int randomNum;
        var random = new Random((uint)UnityEngine.Random.Range(0, 100000));
        randomNum = random.NextInt(0, Lineups.Count);

        var gridType = entityManager.GetBuffer<CoordinatesGridBuffer>(Lineups[randomNum]);


        // Instantiate red team ------------------

        for (int k = 0; k < unit.xGridCount; k++)
        {
            for (int j = 0; j < unit.zGridCount; j++)
            {
                var grid = ecb.Instantiate(unit.gridPrefab);
                float3 GridPostion = new float3(k * unit.xPadding, unit.baseOffset, j * unit.zPadding);
                ecb.SetComponent(grid, new LocalTransform { Position = GridPostion, Scale = 0.1f });
                ecb.SetName(grid, "[" + k + "," + j + "]");
                for (int i = 0; i < gridType.Length; i++)
                {
                    if (k == gridType[i].axis.x && j == gridType[i].axis.y)
                    {
                        var newUnit = ecb.Instantiate(unit.teamRedUnitPrefab);
                        ecb.AddComponent<Target>(newUnit);
                        ecb.AddComponent<RedTeam>(newUnit);
                        float3 UnitPostion = new float3(k * unit.xPadding, unit.baseOffset + 1, j * unit.zPadding);
                        ecb.SetComponent(newUnit, new LocalTransform { Position = UnitPostion, Scale = 1f });
                        ecb.SetName(newUnit, "Red Unit [" + k + "," + j + "]");
                    }
                }
            }
        }
        ecb.Playback(entityManager);


    }
    public void SpawnBlueTeam()
    {

        var unit = SystemAPI.GetAspect<GameDataAspect>(unitEntity);
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var gridType = entityManager.GetBuffer<CoordinatesGridBuffer>(EntityCorrdinates);

        // Instantiate blue team ------------------

        for (int k = 0; k < unit.xGridCount; k++)
        {
            for (int j = 0; j < unit.zGridCount; j++)
            {
                var grid = ecb.Instantiate(unit.gridPrefab);
                ecb.AddComponent<DestroyTag>(grid);
                float3 GridPostion = new float3(k * unit.xPadding, unit.baseOffset, j * unit.zPadding);
                ecb.SetComponent(grid, new LocalTransform { Position = GridPostion + unit.GetteamBlueSpawnPoint().Position, Scale = 0.1f });
                ecb.SetName(grid, "[" + k + "," + j + "]");
                for (int i = 0; i < gridType.Length; i++)
                {
                    if (k == gridType[i].axis.x && j == gridType[i].axis.y)
                    {
                        var newUnit = ecb.Instantiate(unit.teamBlueUnitPrefab);
                        ecb.AddComponent<Target>(newUnit);
                        ecb.AddComponent<BlueTeam>(newUnit);
                        ecb.AddComponent<DestroyTag>(newUnit);
                        float3 UnitPostion = new float3(k * unit.xPadding, unit.baseOffset + 1, j * unit.zPadding);
                        ecb.SetComponent(newUnit, new LocalTransform { Position = UnitPostion + unit.GetteamBlueSpawnPoint().Position, Scale = 1f });
                        ecb.SetName(newUnit, "Blue Unit [" + k + "," + j + "]");
                    }
                }
            }
        }
        ecb.Playback(entityManager);
    }
}
