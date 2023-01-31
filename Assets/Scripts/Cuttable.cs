using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cuttable : MonoBehaviour
    {
        public float cutPercentage = 1;
        private float cutVolume;
        [SerializeField] private bool canBeCutted;
        [SerializeField] private Material upperMaterial;
        [SerializeField] private Material lowerMaterial;

        private void Awake()
        {
            MeshFilter collider = GetComponent<MeshFilter>();
            if (collider)
            {
                cutVolume = FunctionLibrary.VolumeOfMesh(collider.sharedMesh);
                cutPercentage = 1;
            }
            else
            {
                Debug.LogError($"{this.name} doesn't have a mesh collider");
            }
        }

        public Material GetUpperMaterial()
        {
            return upperMaterial;
        }

        public Material GetLowerMaterial()
        {
            return lowerMaterial;
        }

        public float GetPercentage()
        {
            return cutPercentage;
        }
        
        public void SetVolumePercentage(float InVolume, float InPercentage)
        {
            SetCanBeCutted(InPercentage >= 0.10f);
            cutVolume = InVolume;
            cutPercentage = InPercentage;
        }

        public float GetVolume()
        {
            return cutVolume;
        }

        public void SetCanBeCutted(bool InCut)
        {
            canBeCutted = InCut;
        }

        public bool GetCanBeCutted()
        {
            return canBeCutted;
        }
    }
}