using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Collections;

namespace Assets.Scripts
{
    public class WaterInitializer : MonoBehaviour
    {
        [SerializeField] private Mesh fluidMesh;
        [SerializeField] private Material fluidMaterial;

        [SerializeField] private ISpawner spawner;

        private EntityManager activeEntityManager;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
            activeEntityManager = World.Active.EntityManager;
        }

        void Initialize()
        {
            var fluidArchetype = activeEntityManager.CreateArchetype(
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld)
                );

            var fluids = new NativeArray<Entity>(spawner.FluidCount, Allocator.Temp);
            activeEntityManager.CreateEntity(fluidArchetype, fluids);

            for(int i = 0; i<spawner.FluidCount; i++)
            {

            }

            fluids.Dispose();
        }
    }
}
