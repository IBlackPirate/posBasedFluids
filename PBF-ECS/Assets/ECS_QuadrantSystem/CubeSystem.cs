/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

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

public struct CubeData {
    public Entity Entity;
    public float3 Position;
    public float3 DeltaPosition;
    public FluidComponent FluidComponent;
}

public class CubeSystem : ComponentSystem {

    public static NativeMultiHashMap<int, CubeData> cubeMultiHashMap;

    public const int YMultiplier = 1000;
    public const int ZMultiplier = 1000000;
    private const int CubeSize = 2;

    public static int GetPositionHashMapKey(float3 position) {
        return (int) (math.floor(position.x / CubeSize) 
            + (YMultiplier * math.floor(position.y / CubeSize)) 
            + ZMultiplier * math.floor(position.z / CubeSize));
    }

    private static void DebugDrawQuadrant(float3 position) {
        //Vector3 lowerLeft = new Vector3(math.floor(position.x / quadrantCellSize) * quadrantCellSize, math.floor(position.y / quadrantCellSize) * quadrantCellSize);
        //Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+1, +0) * quadrantCellSize);
        //Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+0, +1) * quadrantCellSize);
        //Debug.DrawLine(lowerLeft + new Vector3(+1, +0) * quadrantCellSize, lowerLeft + new Vector3(+1, +1) * quadrantCellSize);
        //Debug.DrawLine(lowerLeft + new Vector3(+0, +1) * quadrantCellSize, lowerLeft + new Vector3(+1, +1) * quadrantCellSize);
        //Debug.Log(GetPositionHashMapKey(position) + " " + position);
    }

    private static int GetEntityCountInHashMap(NativeMultiHashMap<int, CubeData> quadrantMultiHashMap, int hashMapKey) {
        CubeData cubeData;
        NativeMultiHashMapIterator<int> nativeMultiHashMapIterator;
        int count = 0;
        if (quadrantMultiHashMap.TryGetFirstValue(hashMapKey, out cubeData, out nativeMultiHashMapIterator)) {
            do {
                count++;
            } while (quadrantMultiHashMap.TryGetNextValue(out cubeData, ref nativeMultiHashMapIterator));
        }
        return count;
    }

    [BurstCompile]
    private struct SetQuadrantDataHashMapJob : IJobForEachWithEntity<Translation, FluidComponent> {

        public NativeMultiHashMap<int, CubeData>.ParallelWriter quadrantMultiHashMap;

        public void Execute(Entity entity, int index, ref Translation translation, ref FluidComponent fluid) {
            int hashMapKey = GetPositionHashMapKey(translation.Value);
            quadrantMultiHashMap.Add(hashMapKey, new CubeData {
                Entity = entity,
                Position = translation.Value,
                FluidComponent = fluid
            });
        }
    }

    protected override void OnCreate() {
        cubeMultiHashMap = new NativeMultiHashMap<int, CubeData>(0, Allocator.Persistent);
        base.OnCreate();
    }

    protected override void OnDestroy() {
        cubeMultiHashMap.Dispose();
        base.OnDestroy();
    }

    protected override void OnUpdate() {
        EntityQuery entityQuery = GetEntityQuery(typeof(Translation), typeof(FluidComponent));

        cubeMultiHashMap.Clear();
        // Расширяем объем словаря при необходимости
        if (entityQuery.CalculateEntityCount() > cubeMultiHashMap.Capacity) {
            cubeMultiHashMap.Capacity = entityQuery.CalculateEntityCount();
        }

        SetQuadrantDataHashMapJob setQuadrantDataHashMapJob = new SetQuadrantDataHashMapJob {
            quadrantMultiHashMap = cubeMultiHashMap.AsParallelWriter(),
        };

        JobHandle jobHandle = JobForEachExtensions.Schedule(setQuadrantDataHashMapJob, entityQuery);
        jobHandle.Complete();

        //Debug.Log(GetEntityCountInHashMap(quadrantMultiHashMap, GetPositionHashMapKey(UtilsClass.GetMouseWorldPosition())));
    }

}
