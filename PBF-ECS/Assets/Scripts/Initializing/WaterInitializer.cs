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

namespace PBF.Scripts.Initialization
{
    public class WaterInitializer : MonoBehaviour
    {
        [SerializeField] private bool enableCollisions;
        [SerializeField] private float Gravity = 9.8f;
        [SerializeField] private float fluidRadius = .5f;
        [SerializeField] private float fluidAcc = .5f;

        [SerializeField] private Mesh fluidMesh;
        [SerializeField] private Material fluidMaterial;

        [SerializeField] private CubeSpawner spawner;

        [SerializeField] private Data.Bounds bounds;

        private EntityManager activeEntityManager;

        // Start is called before the first frame update
        void Start()
        {
            activeEntityManager = World.Active.EntityManager;
            GameData.Bounds = bounds;
            GameData.Gravity = Gravity;
            GameData.FluidRadius = fluidRadius;
            GameData.EnableCollisions = enableCollisions;
            Initialize();
        }

        void Initialize()
        {
            var fluidArchetype = activeEntityManager.CreateArchetype(
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(FluidComponent)
                );

            var fluids = new NativeArray<Entity>(spawner.FluidCount, Allocator.Temp);
            var positions = spawner.Spawn();

            activeEntityManager.CreateEntity(fluidArchetype, fluids);

            for (int i = 0; i < spawner.FluidCount; i++)
            {
                activeEntityManager.SetComponentData(fluids[i], new Translation() { Value = positions[i] });
                activeEntityManager.SetComponentData(fluids[i], new FluidComponent() { PrevPosition = positions[i], Acceleration = fluidAcc });
                activeEntityManager.SetSharedComponentData(fluids[i], new RenderMesh() { mesh = fluidMesh, material = fluidMaterial });
            }

            fluids.Dispose();
        }
    }
}
