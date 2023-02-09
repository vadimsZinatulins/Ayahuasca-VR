using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cuttable : MonoBehaviour
    {
        public float cutPercentage = 1;
        private float cutVolume;
        [SerializeField] private float MaxCutPercentage = 0.2f;
        [SerializeField] private bool canBeCutted;
        [SerializeField] private Material upperMaterial;
        [SerializeField] private Material lowerMaterial;

        private void Awake()
        {
            MeshFilter collider = GetComponent<MeshFilter>();
            if (collider)
            {
                cutVolume = N_FunctionLibrary.VolumeOfMesh(collider.sharedMesh);
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

        public float GetCutPercentage()
        {
            return cutPercentage;
        }
        
        public float GetMaxCutPercentage()
        {
            return MaxCutPercentage;
        }
        
        public void SetVolumePercentage(float InVolume, float InPercentage, float InMaxCutPercentage)
        {
            MaxCutPercentage = InMaxCutPercentage;
            SetCanBeCutted(InPercentage >= MaxCutPercentage);
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

        public void OnDestroy()
        {
            
        }
    }
}