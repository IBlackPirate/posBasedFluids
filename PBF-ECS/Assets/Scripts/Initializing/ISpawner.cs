using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace PBF.Scripts.Initialization
{
    public interface ISpawner
    {
        int FluidCount { get; }
        float3[] Spawn();
    }
}
