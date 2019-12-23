using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using PBF.Scripts.Components;
using PBF.Scripts.Data;

public class CollisionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (GameData.EnableCollisions)
        {
            Entities.ForEach((ref Translation p1, ref FluidComponent fluid1) =>
            {
                var pos1 = p1.Value;
                var speed = fluid1.Speed;
                Entities.ForEach((ref Translation p2, ref FluidComponent fluid2) =>
                {
                    // Пересекаются ли
                    if (math.lengthsq(pos1 - p2.Value) > 0 && math.lengthsq(pos1 - p2.Value) < GameData.FluidDiameter * GameData.FluidDiameter)
                    {
                        var penetrationDir = math.normalizesafe(p2.Value - pos1);
                        penetrationDir.x += GameData.Random.NextFloat(-.4f, .4f);
                        penetrationDir.z += GameData.Random.NextFloat(-.4f, .4f);
                        penetrationDir.y += GameData.Random.NextFloat(-.3f, .3f);
                        float penetrationDepth = GameData.FluidDiameter - math.length(pos1 - p2.Value);
                        p2.Value += penetrationDir * penetrationDepth * .2f;
                    }
                });
            });
        }
    }
}
