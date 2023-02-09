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
    public UnityEvent<bool> OnGripChange = new UnityEvent<bool>();
    private FixedJoint attachJoint = null;

    protected GameObject ObjectBeingGrabbed;

    private void Awake()
    {
        attachJoint = GetComponent<FixedJoint>();
    }

    private void Start()
    {
        HandController.OnHandUpdate += OnHandUpdate;
        OnGripChange.AddListener(OnGripChanged);
    }

    private void OnHandUpdate(WebXRHandData handData)
    {
        if ((handData.trigger > 0) != oldGripState)
        {
            oldGripState = handData.trigger > 0;
            OnGripChange?.Invoke(oldGripState);
        }
    }

    private void Update()
    {
        if (HandController.GetButton(WebXRController.ButtonTypes.Grip) != oldGripState)
        {
            oldGripState = HandController.GetButton(WebXRController.ButtonTypes.Grip);
            OnGripChanged(oldGripState);
        }
    }

    private void OnGripChanged(bool InValue)
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
}
