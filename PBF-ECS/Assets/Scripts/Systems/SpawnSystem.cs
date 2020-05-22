using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using PBF.Scripts.Components;
using PBF.Scripts.Data;
using PBF.Scripts.Initialization;

public class SpawnSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        var activeEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.ForEach((Entity e, ref FluidSpawnComponent spawnComponent) => {

            var positions = CubeSpawner.Instance.Spawn();

            foreach(var pos in positions)
            {
                var fluid = activeEntityManager.Instantiate(spawnComponent.PrefabEnity);
                activeEntityManager.SetComponentData(fluid, new Translation() { Value = pos });
                activeEntityManager.SetComponentData(fluid, new FluidComponent() { PrevPosition = pos, Acceleration = GameData.FluidAcceleration });
                activeEntityManager.RemoveComponent(fluid, typeof(MovementComponent));
            }

            PostUpdateCommands.RemoveComponent(e, typeof(FluidSpawnComponent));
        });
    }
}
