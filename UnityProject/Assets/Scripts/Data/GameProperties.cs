using Unity.Entities;
using Unity.Mathematics;

public struct GameProperties : IComponentData
{

    public int xGridCount;
    public int zGridCount;
    public float baseOffset;
    public float xPadding;
    public float zPadding;
    public Entity gridPrefab;

    //-----------------

    public float3 teamBlueUnitPostion;
    public float3 teamRedUnitPostion;
    public Entity teamBlueUnitPrefab;
    public Entity teamRedUnitPrefab;



}
