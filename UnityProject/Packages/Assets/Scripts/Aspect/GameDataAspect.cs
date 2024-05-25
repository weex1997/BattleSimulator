
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct GameDataAspect : IAspect
{
    private readonly RefRW<GameProperties> _gridProperties;

    public float3 teamBlueUnitPostion => _gridProperties.ValueRO.teamBlueUnitPostion;
    public float3 teamRedUnitPostion => _gridProperties.ValueRO.teamRedUnitPostion;

    public Entity teamBlueUnitPrefab => _gridProperties.ValueRO.teamBlueUnitPrefab;
    public Entity teamRedUnitPrefab => _gridProperties.ValueRO.teamRedUnitPrefab;


    public int xGridCount => _gridProperties.ValueRO.xGridCount;
    public int zGridCount => _gridProperties.ValueRO.zGridCount;
    public float xPadding => _gridProperties.ValueRO.xPadding;
    public float zPadding => _gridProperties.ValueRO.zPadding;
    public float baseOffset => _gridProperties.ValueRO.baseOffset;
    public Entity gridPrefab => _gridProperties.ValueRO.gridPrefab;


    public LocalTransform GetteamBlueSpawnPoint()
    {
        var position = _gridProperties.ValueRW.teamBlueUnitPostion;
        return new LocalTransform
        {
            Position = position,
            Rotation = quaternion.identity,
            Scale = 1f
        };
    }
    public LocalTransform GetteamRedSpawnPoint()
    {
        var position = _gridProperties.ValueRW.teamRedUnitPostion;
        return new LocalTransform
        {
            Position = position,
            Rotation = quaternion.identity,
            Scale = 1f
        };
    }

}
