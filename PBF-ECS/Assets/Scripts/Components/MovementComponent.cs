using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using PBF.Scripts.Components;
using PBF.Scripts.Data;
using PBF.Scripts.Initialization;

[GenerateAuthoringComponent]
public struct MovementComponent : IComponentData
{
    public float3 MovementOffset;
}
