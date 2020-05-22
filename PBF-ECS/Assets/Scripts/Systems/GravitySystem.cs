using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using PBF.Scripts.Components;
using PBF.Scripts.Data;
using Unity.Jobs;
using Unity.Burst;

namespace PBF.Scripts.Systems
{
    [BurstCompile]
    public class GravitySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ybound = GameData.Bounds.YBottomBound;
            var deltaTime = Time.DeltaTime;
            return Entities.ForEach((ref Translation translation, ref FluidComponent fluid) =>
            {
                fluid.PrevPosition = translation.Value;
                fluid.GravitySpeed += GameData.Gravity;
                fluid.GravitySpeed = math.clamp(fluid.GravitySpeed, 0, 3);
                if (translation.Value.y != ybound)
                    translation.Value += GameData.VectorDown * fluid.GravitySpeed * deltaTime;
            }).Schedule(inputDeps);
        }
    }
}
