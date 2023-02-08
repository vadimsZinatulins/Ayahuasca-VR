using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BS_Grabbable : MonoBehaviour
{
    public UnityEvent OnGrabbed = new UnityEvent();
    public UnityEvent OnReleased = new UnityEvent();
    private Vector3 lastPos;
    public float rbSimulatedVelocity;
    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        rbSimulatedVelocity = (transform.position - lastPos).magnitude;
    }
}
