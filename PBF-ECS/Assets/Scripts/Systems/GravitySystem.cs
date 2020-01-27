using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using PBF.Scripts.Components;
using PBF.Scripts.Data;

namespace PBF.Scripts.Systems
{
    public class GravitySystem : ComponentSystem
    {
        private float deltaTime;
        protected override void OnUpdate()
        {
            deltaTime = Time.deltaTime;
            Entities.ForEach((ref Translation translation, ref FluidComponent fluid) =>
            {
                fluid.PrevPosition = translation.Value;
                fluid.GravitySpeed += GameData.Gravity;
                translation.Value += GameData.VectorDown * fluid.GravitySpeed * deltaTime;
            });
        }
    }
}
