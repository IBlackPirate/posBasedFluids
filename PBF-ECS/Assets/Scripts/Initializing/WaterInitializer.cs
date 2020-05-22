using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Collections;

using PBF.Scripts.Components;
using PBF.Scripts.Systems;
using PBF.Scripts.Data;
using System;

namespace PBF.Scripts.Initialization
{
    public class WaterInitializer : MonoBehaviour
    {
        [SerializeField] private bool enableCollisions;
        [SerializeField] private float Gravity = 9.8f;
        [SerializeField] private float fluidRadius = .5f;
        [SerializeField] private float fluidAcc = .4f;
        [SerializeField] private float density = .4f;

        [SerializeField] private CubeSpawner spawner;

        [SerializeField] private Data.Bounds bounds;

        private EntityManager activeEntityManager;

        // Start is called before the first frame update
        void Start()
        {
            //activeEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            GameData.Bounds = bounds;
            GameData.FluidAcceleration = fluidAcc;
            ////GameData.FluidRadius = fluidRadius;
            //GameData.EnableCollisions = enableCollisions;
            ////GameData.Density = density;
            //Initialize();

            StartCoroutine(MoveBound());
        }

        private IEnumerator MoveBound()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                for (int i = 0; i < 40; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    GameData.Bounds.ZForwardBound += 0.3f;
                }
                yield return new WaitForSeconds(4);
                for (int i = 0; i < 40; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    GameData.Bounds.ZForwardBound -= 0.3f;
                }
            }
        }

        //void Initialize()
        //{
        //    var fluidArchetype = activeEntityManager.CreateArchetype(
        //        typeof(Translation),
        //        typeof(RenderMesh),
        //        typeof(LocalToWorld),
        //        typeof(FluidComponent)
        //        );

        //    var fluids = new NativeArray<Entity>(spawner.FluidCount, Allocator.Temp);
        //    var positions = spawner.Spawn();

        //    activeEntityManager.CreateEntity(fluidArchetype, fluids);

        //    for (int i = 0; i < spawner.FluidCount; i++)
        //    {
        //        activeEntityManager.SetComponentData(fluids[i], new Translation() { Value = positions[i] });
        //        activeEntityManager.SetComponentData(fluids[i], new FluidComponent() { PrevPosition = positions[i], Acceleration = fluidAcc });
        //        activeEntityManager.SetSharedComponentData(fluids[i], new RenderMesh() { mesh = fluidMesh, material = fluidMaterial });
        //    }

        //    fluids.Dispose();
        //}
    }
}
