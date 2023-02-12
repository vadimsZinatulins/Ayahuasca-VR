using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebXR;
using WebXR.Interactions;

public class BS_HandEvents : MonoBehaviour
{
    public WebXRController HandController;
    public ControllerInteraction HandInteraction;
    
    // GRIP
    private bool oldGripState = false;
    public UnityEvent<BS_HandEvents,bool> OnGripChange = new UnityEvent<BS_HandEvents,bool>();
    private FixedJoint attachJoint = null;
    
    // AXIS
    private Vector3 stickValue;

    protected GameObject ObjectBeingGrabbed;

    private void Awake()
    {
        attachJoint = GetComponent<FixedJoint>();
        
        OnGripChange.AddListener(GripChanged);
    }

    private void Start()
    {
        HandController.OnHandUpdate += OnHandUpdate;
    }

    private void OnHandUpdate(WebXRHandData handData)
    {
        //if ((handData.trigger > 0) != oldGripState)
        //{
        //    oldGripState = handData.trigger > 0;
        //    OnGripChange?.Invoke(this, oldGripState);
        //}
    }

    private void Update()
    {
        if (HandController.GetButton(WebXRController.ButtonTypes.Grip) != oldGripState)
        {
            oldGripState = HandController.GetButton(WebXRController.ButtonTypes.Grip);
            GripChanged(this, oldGripState);
            Debug.Log($"Old grip: {oldGripState}");
            OnGripChange?.Invoke(this, oldGripState);
        }

        stickValue = HandController.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick);
    }

    private void GripChanged(BS_HandEvents InHand,bool InValue)
    {
        if (InValue)
        {
            HandInteraction.Pickup();
            if (ObjectBeingGrabbed == null)
            {
                if (attachJoint.connectedBody != null)
                {
                    ObjectBeingGrabbed = attachJoint.connectedBody.gameObject;
                    if (ObjectBeingGrabbed.TryGetComponent(out BS_Grabbable grabbable))
                    {
                        grabbable.OnGrabbed.Invoke();
                    }
                }
            }
        }
        else
        {
            HandInteraction.Drop();
            if (ObjectBeingGrabbed != null)
            {
                if (ObjectBeingGrabbed.TryGetComponent(out BS_Grabbable grabbable))
                {
                    grabbable.OnReleased.Invoke();
                }
                ObjectBeingGrabbed = null;
            }

        }
    }

    public void OverrideHandGrabbable(Rigidbody InRb)
    {
        InRb.MovePosition(transform.position);
        attachJoint.connectedBody = InRb;
    }

    public Vector3 GetThumbstickValue()
    {
        return stickValue;
    }
}
