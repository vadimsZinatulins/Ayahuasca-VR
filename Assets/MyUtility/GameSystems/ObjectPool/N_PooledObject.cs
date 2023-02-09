using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility.ObjectPool
{
    public class N_PooledObject : MonoBehaviour
    {
        public N_ObjectPool owner;
    
    }
    public static class N_PooledGameObjectExtensions
    {
        public static void ReturnToPool(this GameObject gameObject)
        {
            var pooledObject = gameObject.GetComponent<N_PooledObject>();
            if (pooledObject != null)
            {
                if (pooledObject.owner != null)
                {
                    pooledObject.owner.ReturnObject(gameObject);
                }
            }
        }
    }
}