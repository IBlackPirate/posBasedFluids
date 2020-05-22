using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using PBF.Scripts.Components;
using PBF.Scripts.Data;
using PBF.Scripts.Systems;
using Unity.Jobs;

[UpdateAfter(typeof(FindCollisionSystem))]
public class CollisionSystem : JobComponentSystem
{
    //protected override void OnUpdate()
    //{

    //    //HashSet<int> entities = new HashSet<int>();

    //    if (GameData.EnableCollisions)
    //    {
    //        for (int i = 0; i < iterationCount; i++)
    //        {

    //        }
    //    }
    //}

    EntityCommandBuffer.Concurrent eb;

    protected override void OnCreate()
    {
        eb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent();
        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var Bounds = GameData.Bounds;
        return Entities.ForEach((Entity e, ref MovementComponent movement, ref Translation translation) =>
        {
            translation.Value += movement.MovementOffset;
            movement.MovementOffset = float3.zero;

            translation.Value.y = math.clamp(translation.Value.y, Bounds.YBottomBound, Bounds.YTopBound);
            translation.Value.x = math.clamp(translation.Value.x, Bounds.XLeftBound, Bounds.XRightBound);
            translation.Value.z = math.clamp(translation.Value.z, Bounds.ZBackBound, Bounds.ZForwardBound);
            //Debug.Log(movement.MovementOffset);
        }).Schedule(inputDeps);
    }


    //private void OldColls()
    //{
    //    float delta = 0.05f;
    //    bool isRandom = true;
    //    //int iterationCount = 1;

    //    return Entities.ForEach((ref Translation p1, ref FluidComponent fluid1) =>
    //    {
    //        var pos1 = p1.Value;
    //        var tpos = p1.Value;
    //        Entities.ForEach((ref Translation p2, ref FluidComponent fluid2) =>
    //        {
    //            // Расстояние, на котором действует связь
    //            if (math.lengthsq(tpos - p2.Value) > 0 && math.lengthsq(tpos - p2.Value) < 4 * GameData.FluidDiameter * GameData.FluidDiameter)
    //            {
    //                var penetrationDir = math.normalizesafe(p2.Value - tpos);
    //                if (isRandom)
    //                {
    //                    penetrationDir.x += GameData.Random.NextFloat(-delta, delta);
    //                    penetrationDir.z += GameData.Random.NextFloat(-delta, delta);
    //                    penetrationDir.y += GameData.Random.NextFloat(-delta, delta);
    //                }
    //                float penetrationDepth = GameData.FluidDiameter - math.length(tpos - p2.Value);

    //                // Пересекаются ли
    //                if (math.lengthsq(tpos - p2.Value) > 0 && math.lengthsq(tpos - p2.Value) < GameData.FluidDiameter * GameData.FluidDiameter)
    //                {
    //                    p2.Value += penetrationDir * penetrationDepth * .5f;
    //                    pos1 += -penetrationDir * penetrationDepth * .5f;
    //                }
    //                else // Просто действует связь
    //                {
    //                    p2.Value += penetrationDir * .5f * GameData.Density;
    //                    pos1 += -penetrationDir * .5f * GameData.Density;
    //                }

    //                p2.Value.y = math.clamp(p2.Value.y, GameData.Bounds.YBottomBound, GameData.Bounds.YTopBound);
    //                p2.Value.x = math.clamp(p2.Value.x, GameData.Bounds.XLeftBound, GameData.Bounds.XRightBound);
    //                p2.Value.z = math.clamp(p2.Value.z, GameData.Bounds.ZBackBound, GameData.Bounds.ZForwardBound);

    //                pos1.y = math.clamp(pos1.y, GameData.Bounds.YBottomBound, GameData.Bounds.YTopBound);
    //                pos1.x = math.clamp(pos1.x, GameData.Bounds.XLeftBound, GameData.Bounds.XRightBound);
    //                pos1.z = math.clamp(pos1.z, GameData.Bounds.ZBackBound, GameData.Bounds.ZForwardBound);
    //            }
    //        });
    //        p1.Value = pos1;
    //    }).Schedule(inputDeps);
    //}
}
