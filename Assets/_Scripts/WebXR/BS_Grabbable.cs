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
    [SerializeField] private float positionLerpSpeed = 3f;
    [SerializeField] private float SpeedMultiplier = 20;
    public float rbSimulatedVelocity;
    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 lerpedPosition = Vector3.Lerp(lastPos, transform.position, Time.deltaTime * positionLerpSpeed);
        rbSimulatedVelocity = (lerpedPosition - lastPos).magnitude * SpeedMultiplier;
        //rbSimulatedVelocity = (transform.position - lastPos).magnitude * SpeedMultiplier;
        lastPos = transform.position;
    }
}
