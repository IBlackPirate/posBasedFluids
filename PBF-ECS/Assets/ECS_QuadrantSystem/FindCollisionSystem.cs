
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using PBF.Scripts.Components;
using PBF.Scripts.Data;
using Bounds = PBF.Scripts.Data.Bounds;

[UpdateAfter(typeof(CubeSystem))]
public class FindCollisionSystem : JobComponentSystem
{
    [BurstCompile]
    private struct FindCollisionCubeSystemJob : IJobForEachWithEntity<Translation, FluidComponent>
    {

        [ReadOnly] public NativeMultiHashMap<int, CubeData> quadrantMultiHashMap;
        [ReadOnly] public Bounds Bounds;
        public NativeArray<float3> finalDirectionsArray;

        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref FluidComponent fluidComponent)
        {
            float3 offset = float3.zero;
            int hashMapKey = CubeSystem.GetPositionHashMapKey(translation.Value);

            for (int i = -1; i < 2; i++)
            {
                ApplyCollisions(hashMapKey, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey + 1, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey - 1, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey + CubeSystem.YMultiplier, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey - CubeSystem.YMultiplier, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey + 1 + CubeSystem.YMultiplier, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey - 1 + CubeSystem.YMultiplier, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey + 1 - CubeSystem.YMultiplier, entity, ref translation, ref offset);
                ApplyCollisions(hashMapKey - 1 - CubeSystem.YMultiplier, entity, ref translation, ref offset);

                hashMapKey += CubeSystem.ZMultiplier;
            }
            //finalDirectionsArray[index] = offset;
        }


        private void ApplyCollisions(int hashMapKey, Entity original, ref Translation translation, ref float3 offset)
        {
            float delta = 0.05f;
            CubeData quadrantData;
            NativeMultiHashMapIterator<int> nativeMultiHashMapIterator;
            if (quadrantMultiHashMap.TryGetFirstValue(hashMapKey, out quadrantData, out nativeMultiHashMapIterator))
            {
                do
                {
                    if (quadrantData.Entity.Index != original.Index
                        && math.lengthsq(translation.Value - quadrantData.Position) < 4 * GameData.FluidDiameter * GameData.FluidDiameter)
                    {
                        var centerDirection = translation.Value - quadrantData.Position;
                        var penetrationDir = math.normalizesafe(centerDirection);

                        penetrationDir.x += GameData.Random.NextFloat(-delta, delta);
                        penetrationDir.z += GameData.Random.NextFloat(-delta, delta);
                        penetrationDir.y += GameData.Random.NextFloat(-delta, delta);

                        // Пересекаются ли
                        if (math.lengthsq(centerDirection) < GameData.FluidDiameter * GameData.FluidDiameter)
                        {
                            float penetrationDepth = GameData.FluidDiameter - math.length(centerDirection);
                            //offset += penetrationDir * penetrationDepth * .5f;
                            translation.Value += penetrationDir * penetrationDepth * .5f;
                        }
                        else // Просто действует связь
                        {
                            //offset += penetrationDir * .01f * GameData.Density;
                            translation.Value -= penetrationDir * .01f * GameData.Density;
                        }
                    }
                } while (quadrantMultiHashMap.TryGetNextValue(out quadrantData, ref nativeMultiHashMapIterator));
            }
        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityQuery unitQuery = GetEntityQuery(typeof(FluidComponent));
        NativeArray<float3> directionsArray = new NativeArray<float3>(unitQuery.CalculateEntityCount(), Allocator.TempJob);

        FindCollisionCubeSystemJob findTargetQuadrantSystemJob = new FindCollisionCubeSystemJob
        {
            quadrantMultiHashMap = CubeSystem.cubeMultiHashMap,
            finalDirectionsArray = directionsArray,
            Bounds = GameData.Bounds
        };
        JobHandle jobHandle = findTargetQuadrantSystemJob.Schedule(this, inputDeps);


        // Add HasTarget Component to Entities that have a Closest Target
        //AddComponentJob addComponentJob = new AddComponentJob
        //{
        //    offsetsArray = directionsArray,
        //    entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
        //};
        //jobHandle = addComponentJob.Schedule(this, jobHandle);

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }

    // Add HasTarget Component to Entities that have a Closest Target
    private struct AddComponentJob : IJobForEachWithEntity<Translation, FluidComponent>
    {
        [DeallocateOnJobCompletion, ReadOnly] public NativeArray<float3> offsetsArray;
        public EntityCommandBuffer.Concurrent entityCommandBuffer;

        public void Execute(Entity entity, int index, ref Translation translation, ref FluidComponent fluid)
        {
            if (offsetsArray[index].x != 0 || offsetsArray[index].y != 0 || offsetsArray[index].z != 0)
            {
                entityCommandBuffer.RemoveComponent(index, entity, typeof(MovementComponent));
                entityCommandBuffer.AddComponent(index, entity, new MovementComponent { MovementOffset = offsetsArray[index] });
            }
        }
    }
}

