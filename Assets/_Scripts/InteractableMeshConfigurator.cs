using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMeshConfigurator : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    public void Setup(Mesh InMesh, Material[] InMaterial)
    {
        meshFilter.mesh = InMesh;
        meshRenderer.materials = InMaterial;
        meshCollider.sharedMesh = InMesh;
        meshCollider.convex = true;
    }
}
