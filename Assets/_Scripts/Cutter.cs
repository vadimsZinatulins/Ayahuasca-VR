using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using EzySlice;
using MyUtility.Database;
using UnityEngine;

//using Plane = UnityEngine.Plane;

public class Cutter : MonoBehaviour
{
    [SerializeField] private CutterBlade cutterBlade;

    [SerializeField] private BS_Grabbable _grabbable;
    //public Plane m_Plane;
    //private RaycastHit hit;
    [SerializeField] private float cutForceMinimum;
    [SerializeField] private bool isEquipped = false;
    
    //Debug
    private int debugId;

    private void Start()
    {
        if (TextDebugger.Instance != null && _grabbable != null)
        {
            TextDebugger.Instance.AddScreenDebug("Cutter setted up",0);
            debugId = TextDebugger.Instance.AddScreenDebug($"Cuttables near: {cutterBlade.GetCuttables().Count} \n rb velocity: {_grabbable.rbSimulatedVelocity}", 1, true);
        }
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (TextDebugger.Instance != null)
        {
            TextDebugger.Instance.UpdateDebugText(debugId,$"Cuttables near: {cutterBlade.GetCuttables().Count} \n rb velocity: {_grabbable.rbSimulatedVelocity}");
        }

        if (isEquipped)
        {
            MainDataAsset DataAsset = N_FunctionLibrary.GetMainDataAsset();
            if (DataAsset != null)
            {
                var cuttables = cutterBlade.GetCuttables();
                foreach (var c in cuttables)
                {
                    if (c.GetCanBeCutted() && cutForceMinimum <= _grabbable.rbSimulatedVelocity)
                    {
                        MeshFilter mesh = c.GetComponent<MeshFilter>();
                        Coockable coockable = c.GetComponent<Coockable>();
                        if (mesh && 
                            c    && 
                            coockable)
                        {
                            // Size of this piece
                            float volumeCuttable = N_FunctionLibrary.VolumeOfMesh(mesh.mesh);
                            
                            EzySlice.Plane cuttingPlane = new EzySlice.Plane();
                            
                            cuttingPlane.Compute(cutterBlade.lastPos, cutterBlade.transform.up);
                            
                            //SlicedHull Slicer = c.gameObject.Slice(cuttingPlane,c.GetLowerMaterial());
                            SlicedHull Slicer = c.gameObject.Slice(cutterBlade.lastPos, cutterBlade.transform.up);
                            if (Slicer != null)
                            {
                                //--------------------------------UPPER--------------------------------------
                                GameObject upperHull = Slicer.CreateUpperHull(c.gameObject, c.GetUpperMaterial());
                                upperHull.tag = "Interactable";
                                //MeshRenderer upperRenderer = upperHull.GetComponent<MeshRenderer>();
                                MeshCollider upperCollider = upperHull.AddComponent<MeshCollider>();
                                upperCollider.convex = true;
                                upperHull.AddComponent<Rigidbody>();
                                
                                float upperVolume = N_FunctionLibrary.VolumeOfMesh(upperCollider.sharedMesh);
                                Coockable upperCut = upperHull.AddComponent<Coockable>();
                                upperCut.SetCookColor(coockable.GetCookColor());
                                upperCut.SetIngredientType(coockable.GetIngredientType());
                                upperCut.SetVolumePercentage(upperVolume,(upperVolume/volumeCuttable)*c.GetCutPercentage(),c.GetMaxCutPercentage());
                                
                                //InteractableFacade upperFacade = Instantiate(DataAsset.GetEmptyInteractorPrefab(), upperHull.transform.position, upperHull.transform.rotation).GetComponent<InteractableFacade>();
                                //InteractableMeshConfigurator upperMeshConfigurator = upperFacade.MeshContainer.GetComponentInChildren<InteractableMeshConfigurator>();
                                //upperFacade.transform.SetGlobalScale(c.transform.lossyScale);
                                //upperFacade.transform.position = c.transform.position;
                                //if (upperMeshConfigurator != null)
                                //{
                                //    upperMeshConfigurator.Setup(upperCollider.sharedMesh, upperRenderer.materials);
                                //    
                                //    float upperVolume = N_FunctionLibrary.VolumeOfMesh(upperCollider.sharedMesh);
                                //    Cuttable upperCut = upperHull.gameObject.AddComponent<Cuttable>();
                                //    upperCut.SetVolumePercentage(upperVolume,(upperVolume/volumeCuttable)*c.GetPercentage());
                                //}
                                //--------------------------------UPPER--------------------------------------
                                
                                //---------------------------------LOWER-------------------------------------
                                GameObject bottomHull = Slicer.CreateLowerHull(c.gameObject, c.GetLowerMaterial());
                                bottomHull.tag = "Interactable";
                                //MeshRenderer bottomRenderer = bottomHull.GetComponent<MeshRenderer>();
                                MeshCollider bottomCollider = bottomHull.AddComponent<MeshCollider>();
                                bottomCollider.convex = true;
                                bottomHull.AddComponent<Rigidbody>();
                                
                                float bottomVolume = N_FunctionLibrary.VolumeOfMesh(bottomCollider.sharedMesh);
                                Coockable bottomCut = bottomHull.AddComponent<Coockable>();
                                bottomCut.SetCookColor(coockable.GetCookColor());
                                bottomCut.SetIngredientType(coockable.GetIngredientType());
                                bottomCut.SetVolumePercentage(bottomVolume,(bottomVolume/volumeCuttable)*c.GetCutPercentage(),c.GetMaxCutPercentage());
                                
                                //InteractableFacade bottomFacade = Instantiate(DataAsset.GetEmptyInteractorPrefab(), bottomHull.transform.position, bottomHull.transform.rotation).GetComponent<InteractableFacade>();
                                //InteractableMeshConfigurator bottomMeshConfigurator = bottomFacade.MeshContainer.GetComponentInChildren<InteractableMeshConfigurator>();
                                //bottomFacade.transform.SetGlobalScale(c.transform.lossyScale);
                                //bottomFacade.transform.position = c.transform.position;
                                //if (bottomMeshConfigurator != null)
                                //{
                                //    bottomMeshConfigurator.Setup(bottomCollider.sharedMesh, bottomRenderer.materials);
                                //    
                                //    float bottomVolume = N_FunctionLibrary.VolumeOfMesh(bottomCollider.sharedMesh);
                                //    Cuttable bottomCut = bottomMeshConfigurator.gameObject.AddComponent<Cuttable>();
                                //    bottomCut.SetVolumePercentage(bottomVolume,(bottomVolume/volumeCuttable)*c.GetPercentage());
                                //}

                                Debug.Log($"Cutted object: {c.name}");
                                //---------------------------------LOWER-------------------------------------
                                Destroy(c.gameObject);
                                

                            }
                        }
                        else
                        {
                            Debug.LogError("Didn't find mesh renderer on cutter",this);
                        }
                    }
                }
            }
        }
        
        
        //TEST 
        /*
        var cuttables = cutterBlade.GetCuttables();
        foreach (var c in cuttables)
        {
            if (c.GetCanBeCutted())
            {
                MeshFilter mesh = c.GetComponent<MeshFilter>();
                if (mesh && c)
                {
                    // Size of this piece
                    float volumeCuttable = FunctionLibrary.VolumeOfMesh(mesh.mesh);
                    
                    EzySlice.Plane cuttingPlane = new EzySlice.Plane();
                    
                    cuttingPlane.Compute(cutterBlade.gameObject);
                    
                    //SlicedHull Slicer = c.gameObject.Slice(cuttingPlane,c.GetLowerMaterial());
                    SlicedHull Slicer = c.gameObject.Slice(cuttingPlane);
                    if (Slicer != null)
                    {
                        //----------------------------------------------------------------------
                        GameObject upper = Slicer.CreateUpperHull(c.gameObject, c.GetUpperMaterial());
                        upper.AddComponent<Rigidbody>();
                    
                        MeshCollider UpperCollider = upper.AddComponent<MeshCollider>();
                        UpperCollider.convex = true;
                        float UpperVolume = FunctionLibrary.VolumeOfMesh(UpperCollider.sharedMesh);
                    
                        Cuttable UpperCut = upper.AddComponent<Cuttable>();
                        UpperCut.SetVolumePercentage(UpperVolume,(UpperVolume/volumeCuttable)*c.GetPercentage());
                    
                        upper.layer = LayerMask.NameToLayer("Cuttable");
                        //----------------------------------------------------------------------
                        GameObject bottom = Slicer.CreateLowerHull(c.gameObject, c.GetLowerMaterial());
                        bottom.AddComponent<Rigidbody>();
                    
                        MeshCollider BottomCollider = bottom.AddComponent<MeshCollider>();
                        BottomCollider.convex = true;
                        float BottomVolume = FunctionLibrary.VolumeOfMesh(BottomCollider.sharedMesh);
                    
                        Cuttable BottomCut = bottom.AddComponent<Cuttable>();
                        BottomCut.SetVolumePercentage(BottomVolume,(BottomVolume/volumeCuttable)*c.GetPercentage());
                    
                        bottom.layer = LayerMask.NameToLayer("Cuttable");
                    
                    
                        Destroy(c.gameObject);
                    }
                }
                else
                {
                    Debug.LogError("Didn't find mesh renderer on cutter",this);
                }
            }
        }
        */
            /*
            if (Physics.BoxCast(transform.position,new Vector3(1,0.5f,0.5f),transform.up,out hit,transform.rotation,1f,cutLayer))
            {
                objectToSlice = hit.transform.gameObject;
                
                SlicedHull Slicer = Slice(objectToSlice, material);
                GameObject upper = Slicer.CreateUpperHull(objectToSlice, material);
                upper.AddComponent<MeshCollider>().convex = true;
                upper.AddComponent<Rigidbody>();
                upper.layer = LayerMask.NameToLayer("Default");
                GameObject bottom = Slicer.CreateLowerHull(objectToSlice, material);
                bottom.AddComponent<MeshCollider>().convex = true;
                bottom.AddComponent<Rigidbody>();
                bottom.layer = LayerMask.NameToLayer("Default");
                Destroy(objectToSlice);
                objectToSlice = null;
            }
            */
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.TryGetComponent<Cuttable>(out Cuttable c))
        {
            if (c.GetCanBeCutted() && cutForceMinimum<=rb.velocity.magnitude)
            {
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                Cuttable cuttable = GetComponent<Cuttable>();
                if (mesh && cuttable)
                {
                    // Size of this piece
                    float magnitude = mesh.bounds.size.magnitude;
                    
                    SlicedHull Slicer = c.gameObject.Slice(cutterBlade.transform.position, cutterBlade.transform.forward);
                    //----------------------------------------------------------------------
                    GameObject upper = Slicer.CreateUpperHull(c.gameObject, c.GetUpperMaterial());
                    upper.AddComponent<Rigidbody>();
                    
                    MeshCollider UpperCollider = upper.AddComponent<MeshCollider>();
                    UpperCollider.convex = true;
                    float Uppermagnitude = UpperCollider.bounds.size.magnitude;
                    
                    Cuttable UpperCut = upper.AddComponent<Cuttable>();
                    UpperCut.SetPercentage((Uppermagnitude/magnitude)*cuttable.GetPercentage());
                    
                    upper.layer = LayerMask.NameToLayer("Cuttable");
                    //----------------------------------------------------------------------
                    GameObject bottom = Slicer.CreateLowerHull(c.gameObject, c.GetLowerMaterial());
                    bottom.AddComponent<Rigidbody>();
                    
                    MeshCollider BottomCollider = bottom.AddComponent<MeshCollider>();
                    BottomCollider.convex = true;
                    float Bottommagnitude = BottomCollider.bounds.size.magnitude;
                    
                    Cuttable BottomCut = bottom.AddComponent<Cuttable>();
                    BottomCut.SetPercentage((Bottommagnitude/magnitude)*cuttable.GetPercentage());
                    
                    bottom.layer = LayerMask.NameToLayer("Cuttable");
                    
                    
                    Destroy(c.gameObject);
                }
                else
                {
                    Debug.LogError("Didn't find mesh renderer on cutter",this);
                }
            }
        }
        */
    }

    public bool GetIsEquipped()
    {
        return isEquipped;
    }

    public void SetIsEquipped(bool equipped)
    {
        if (TextDebugger.Instance != null)
        {
            TextDebugger.Instance.AddScreenDebug(equipped ? $"Equipped <color=#00AA00>{this.name}</color>" : $"Unequipped <color=#AA0000>{this.name}</color>",0);
        }
        isEquipped = equipped;
    }
    

}
