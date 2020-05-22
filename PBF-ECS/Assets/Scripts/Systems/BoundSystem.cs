using PBF.Scripts.Components;
using PBF.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;

//[UpdateAfter(typeof(CollisionSystem))]
[BurstCompile]
public class BoundSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var bounds = GameData.Bounds;
        return Entities.ForEach((ref Translation translation, ref FluidComponent fluid) =>
        {
            translation.Value.y = math.clamp(translation.Value.y, bounds.YBottomBound, bounds.YTopBound);
            translation.Value.x = math.clamp(translation.Value.x, bounds.XLeftBound, bounds.XRightBound);
            translation.Value.z = math.clamp(translation.Value.z, bounds.ZBackBound, bounds.ZForwardBound);
        }).Schedule(inputDeps);
    }
}
