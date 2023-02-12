using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BS_Grabbable : MonoBehaviour
{
    public bool doesMove = true;
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
        if (doesMove)
        {
            Vector3 lerpedPosition = Vector3.Lerp(lastPos, transform.position, Time.deltaTime * positionLerpSpeed);
            rbSimulatedVelocity = (lerpedPosition - lastPos).magnitude * SpeedMultiplier;
            //rbSimulatedVelocity = (transform.position - lastPos).magnitude * SpeedMultiplier;
            lastPos = transform.position;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        BS_HandEvents hand = other.gameObject.GetComponentInParent<BS_HandEvents>();
        if (hand)
        {
            Debug.Log($"Added listener to {hand.name}");
            hand.OnGripChange.AddListener(OnPlayerGrip);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        BS_HandEvents hand = other.gameObject.GetComponentInParent<BS_HandEvents>();
        if (hand)
        {
            Debug.Log($"Removed listener to {hand.name}");
            hand.OnGripChange.RemoveListener(OnPlayerGrip);
        }
    }

    // For when the player grips inside this object ( in case its a trigger instead of a collider)
    public virtual void OnPlayerGrip(BS_HandEvents hand, bool InValue)
    {
        
    }
}
