using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace PBF.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct FluidComponent : IComponentData
    {
        public float3 PrevPosition;
        public float3 Speed;
        public float GravitySpeed;
        public float Acceleration;
    }
}