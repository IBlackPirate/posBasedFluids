using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using PBF.Scripts.Components;
using Unity.Jobs;
using Unity.Burst;

namespace PBF.Scripts.Systems {
    [BurstCompile]
    public class MovingSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            return Entities.ForEach((ref Translation translation, ref FluidComponent fluid) =>
            {
                fluid.Speed = translation.Value - fluid.PrevPosition;
                translation.Value += fluid.Speed * fluid.Acceleration * deltaTime * deltaTime;
            }).Schedule(inputDeps);
        }
    }
}
