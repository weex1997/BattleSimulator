using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(5)]
public struct CoordinatesGridBuffer : IBufferElementData
{
    public float2 axis;
}
