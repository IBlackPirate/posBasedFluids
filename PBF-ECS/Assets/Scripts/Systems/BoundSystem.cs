using PBF.Scripts.Components;
using PBF.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class BoundSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref FluidComponent fluid) =>
        {
            if (translation.Value.y >= GameData.Bounds.YTopBound)
                OnExitBound(ref translation.Value.y, GameData.Bounds.YTopBound, ref fluid);
            else if (translation.Value.y <= GameData.Bounds.YBottomBound)
            {
                OnExitBound(ref translation.Value.y, GameData.Bounds.YBottomBound, ref fluid);
                translation.Value.y = GameData.Bounds.YBottomBound;
            }
            if (translation.Value.x >= GameData.Bounds.XRightBound)
                OnExitBound(ref translation.Value.x, GameData.Bounds.XRightBound, ref fluid);
            else if (translation.Value.x <= GameData.Bounds.XLeftBound)
                OnExitBound(ref translation.Value.x, GameData.Bounds.XLeftBound, ref fluid);
            if (translation.Value.z <= GameData.Bounds.ZBackBound)
                OnExitBound(ref translation.Value.z, GameData.Bounds.ZBackBound, ref fluid);
            else if (translation.Value.z >= GameData.Bounds.ZForwardBound)
                OnExitBound(ref translation.Value.z, GameData.Bounds.ZForwardBound, ref fluid);
        });
    }

    private void OnExitBound(ref float fluidPosition, float boundPosition, ref FluidComponent fluid)
    {
        //fluidPosition = boundPosition;
        fluid.Speed *= -0.3f;
    }
}
