using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GameDataMono : MonoBehaviour
{
    public float3 teamBlueUnitPostion;
    public float3 teamRedUnitPostion;
    public GameObject teamBlueUnitPrefab;
    public GameObject teamRedUnitPrefab;

    //---------------------------------

    public int xGridCount;
    public int zGridCount;
    public float baseOffset;
    public float xPadding;
    public float zPadding;
    public GameObject gridPrefab;
}

public class GameDataBaker : Baker<GameDataMono>
{
    public override void Bake(GameDataMono authoring)
    {
        var e = GetEntity(authoring, TransformUsageFlags.NonUniformScale | TransformUsageFlags.Dynamic);


        AddComponent(e, new GameProperties
        {
            xGridCount = authoring.xGridCount,
            zGridCount = authoring.zGridCount,
            baseOffset = authoring.baseOffset,
            xPadding = authoring.xPadding,
            zPadding = authoring.zPadding,
            gridPrefab = GetEntity(authoring.gridPrefab, TransformUsageFlags.Dynamic),


            teamBlueUnitPostion = authoring.teamBlueUnitPostion,
            teamRedUnitPostion = authoring.teamRedUnitPostion,
            teamBlueUnitPrefab = GetEntity(authoring.teamBlueUnitPrefab, TransformUsageFlags.Dynamic),
            teamRedUnitPrefab = GetEntity(authoring.teamRedUnitPrefab, TransformUsageFlags.Dynamic)

        });

    }
}
