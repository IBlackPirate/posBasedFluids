using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using PBF.Scripts.Components;


namespace PBF.Scripts.Systems {
    public class MovingSystem : ComponentSystem
    {
        private float deltaTime;
        protected override void OnUpdate()
        {
            deltaTime = Time.deltaTime;
            Entities.ForEach((ref Translation translation, ref FluidComponent fluid) => {
                var delta = translation.Value - fluid.PrevPosition;
                translation.Value += delta * fluid.Acceleration * deltaTime * deltaTime;
            });
        }
    }
}
