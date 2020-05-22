using PBF.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace PBF.Scripts.Initialization
{
    public class CubeSpawner : MonoBehaviour, ISpawner
    {
        public static CubeSpawner Instance;

        public int FluidCount => HeightCount * WidthCount * LenghtCount;

        [SerializeField] private int HeightCount;
        [SerializeField] private int WidthCount;
        [SerializeField] private int LenghtCount;

        float3 pos;

        private void Awake()
        {
            Instance = this;
            pos = new float3(transform.position.x, transform.position.y, transform.position.z);
        }

        public float3[] Spawn()
        {
            var result = new List<float3>(FluidCount);
            for (int i = 0; i < WidthCount; i++)
                for (int j = 0; j < HeightCount; j++)
                    for (int k = 0; k < LenghtCount; k++)
                        result.Add(new float3(i * GameData.FluidDiameter, j * GameData.FluidDiameter, k * GameData.FluidDiameter) + pos);
            return result.ToArray();
        }
    }
}
