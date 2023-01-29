using System;
using UnityEngine;
using Zinnia.Action;
using WebXR;

public class WebMovementY : FloatAction
{
    public WebXRController controller;
    private float yAxis;

    private void Update()
    {
        var vector2 = controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick);
        yAxis = vector2.y;
        Receive(yAxis);
    }
}