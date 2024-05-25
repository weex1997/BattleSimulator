using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class LineupsMono : MonoBehaviour
{
    public UnitsCoordinatesConfiguration unitsCoordinatesConfiguration;
}

class LineupsBaker : Baker<LineupsMono>
{
    public override void Bake(LineupsMono authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);


        DynamicBuffer<CoordinatesGridBuffer> pathBuffer = AddBuffer<CoordinatesGridBuffer>(entity);
        foreach (float2 axis in authoring.unitsCoordinatesConfiguration.Coordinates)
        {
            pathBuffer.Add(new CoordinatesGridBuffer() { axis = axis });
        }

        //add Lineup tag for the entity
        AddComponent(entity, new Lineup());

        DependsOn(authoring.unitsCoordinatesConfiguration);
    }
}
