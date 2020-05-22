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

public class UnitMoveToTargetSystem : ComponentSystem {

    protected override void OnUpdate() {
        //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //Entities.ForEach((Entity unitEntity, ref HasTarget hasTarget, ref Translation translation) => {
        //    if (entityManager.Exists(hasTarget.targetEntity)) {
        //        Translation targetTranslation = entityManager.GetComponentData<Translation>(hasTarget.targetEntity);

        //        float3 targetDir = math.normalize(targetTranslation.Value - translation.Value);
        //        float moveSpeed = 1f;
        //        translation.Value += targetDir * moveSpeed * Time.DeltaTime;

        //        if (math.distancesq(translation.Value, targetTranslation.Value) < .05f) {
        //            // Close to target, destroy it
        //            PostUpdateCommands.DestroyEntity(hasTarget.targetEntity);
        //            PostUpdateCommands.RemoveComponent(unitEntity, typeof(HasTarget));
        //        }
        //    } else {
        //        // Target Entity already destroyed
        //        PostUpdateCommands.RemoveComponent(unitEntity, typeof(HasTarget));
        //    }
        //});
    }

}
