using System;
using Zinnia.Action;
using WebXR;

public class WebGrip : BooleanAction
{
    public WebXRController controller;

    private void Update()
    {
        Receive(controller.GetButton(WebXRController.ButtonTypes.Grip));
    }
}
