using System;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using UnityEngine;

namespace HurricaneVR.Framework.Components
{
    /// <summary>
    /// Tags a grabbable object as climbable which is then used by the player controller to know if they can climb or not.
    /// For the hexabody integration it will kick in the climbing strength override set on the HexaHands component.
    /// </summary>
    public class HVRClimbable : MonoBehaviour
    {
        private HVRGrabbable _grabbable;
        private void Awake()
        {
            _grabbable = GetComponent<HVRGrabbable>();
            if (_grabbable != null)
            {
                _grabbable.HandFullReleased.AddListener(OnRemoveHand);
            }
        }

        public void OnRemoveHand(HVRHandGrabber InHand, HVRGrabbable InGrababble)
        {
            Vector3 ThrowVelocity = InGrababble.GetAverageVelocity(3,0);
            InHand.Controller.Velocity += ThrowVelocity;
        }
    }
}
