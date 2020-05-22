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
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

public class GameHandler : MonoBehaviour {

    public static GameHandler instance;

    public bool useQuadrantSystem;

    [SerializeField] private Material unitMaterial;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Mesh quadMesh;

    private static EntityManager entityManager;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int i = 0; i < 4000; i++) {
            SpawnUnitEntity();
        }

        for (int i = 0; i < 80000; i++) {
            SpawnTargetEntity();
        }
    }

    private float spawnTargetTimer;

    private void Update() {
        //return;
        spawnTargetTimer -= Time.deltaTime;
        if (spawnTargetTimer < 0) {
            spawnTargetTimer = .1f;
            
            for (int i = 0; i < 500; i++) {
                SpawnTargetEntity();
            }
        }
    }

    private void SpawnUnitEntity() {
        SpawnUnitEntity(new float3(UnityEngine.Random.Range(-100, +100f), UnityEngine.Random.Range(-80, +80f), 0));
    }

    private void SpawnUnitEntity(float3 position) {
        //Entity entity = entityManager.CreateEntity(
        //    typeof(Translation),
        //    typeof(LocalToWorld),
        //    typeof(RenderMesh),
        //    typeof(Scale),
        //    typeof(Unit),
        //    typeof(QuadrantEntity)
        //);
        //SetEntityComponentData(entity, position, quadMesh, unitMaterial);
        //entityManager.SetComponentData(entity, new Scale { Value = 1.5f });
        //entityManager.SetComponentData(entity, new QuadrantEntity { typeEnum = QuadrantEntity.TypeEnum.Unit });
    }

    private void SpawnTargetEntity() {
        //Entity entity = entityManager.CreateEntity(
        //    typeof(Translation),
        //    typeof(LocalToWorld),
        //    typeof(RenderMesh),
        //    typeof(Scale),
        //    typeof(Target),
        //    typeof(QuadrantEntity)
        //);
        //SetEntityComponentData(entity, new float3(UnityEngine.Random.Range(-100, +100f), UnityEngine.Random.Range(-80, +80f), 0), quadMesh, targetMaterial);
        //entityManager.SetComponentData(entity, new Scale { Value = .5f });
        //entityManager.SetComponentData(entity, new QuadrantEntity { typeEnum = QuadrantEntity.TypeEnum.Target });
    }

    private void SetEntityComponentData(Entity entity, float3 spawnPosition, Mesh mesh, Material material) {
        entityManager.SetSharedComponentData<RenderMesh>(entity,
            new RenderMesh {
                material = material,
                mesh = mesh,
            }
        );

        entityManager.SetComponentData<Translation>(entity, 
            new Translation { 
                Value = spawnPosition
            }
        );
    }

}








