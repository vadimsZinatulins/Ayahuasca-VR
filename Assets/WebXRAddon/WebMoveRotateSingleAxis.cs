using System;
using Zinnia.Action;
using WebXR;

public class WebMoveRotateSingleAxis : FloatAction
{
    public bool isX;
    public WebXRController controller;
    private float axisValue;

    private void Update()
    {
        var vector2 = controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick);
        if (isX)
        {
            axisValue = vector2.x;
        }
        else
        {
            axisValue = vector2.y;
        }
        Receive(axisValue);
    }
}