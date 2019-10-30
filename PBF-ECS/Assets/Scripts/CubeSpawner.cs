using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public class CubeSpawner : MonoBehaviour, ISpawner
    {
        public int FluidCount => HeightCount * WidthCount * LenghtCount;

        [SerializeField] private int HeightCount;
        [SerializeField] private int WidthCount;
        [SerializeField] private int LenghtCount;
        [SerializeField] private float Radius = .5f;

        private float Diameter { get { return 2 * Radius; } }

        public Vector3[] Spawn()
        {
            Vector3[] result = new Vector3[FluidCount];
            for (int i = 0; i < WidthCount; i++)
                for (int j = 0; j < HeightCount; j++)
                    for (int k = 0; k < LenghtCount; k++)
                        result[i] = new Vector3(i * Diameter, j * Diameter, k * Diameter);
            return result;
        }
    }
}
