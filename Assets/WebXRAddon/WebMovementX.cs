using System;
using Zinnia.Action;
using WebXR;

public class WebMovementX : FloatAction
{
    public WebXRController controller;
    private float xAxis;

    private void Update()
    {
        var vector2 = controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick);
        xAxis = vector2.x;
        Receive(xAxis);
    }
}