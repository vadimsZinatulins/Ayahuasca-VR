using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

public class BS_Hand : MonoBehaviour
{
    public WebXRController Controller;
    // Start is called before the first frame update
    void Start()
    {
        Controller.OnHandUpdate+= OnHandUpdate;
    }

    private void OnHandUpdate(WebXRHandData obj)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
