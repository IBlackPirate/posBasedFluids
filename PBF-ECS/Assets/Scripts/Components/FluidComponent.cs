using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace PBF.Scripts.Components
{
    public struct FluidComponent : IComponentData
    {
        public float3 PrevPosition;
        public float Speed;
        public float Acceleration;
    }
}