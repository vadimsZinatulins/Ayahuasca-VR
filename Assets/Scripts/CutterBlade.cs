using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CutterBlade : MonoBehaviour
    {
        public Vector3 lastPos;
        [SerializeField]
        private float lerpValue = 1f;
        public Vector3 boxSize;

        private void OnDrawGizmos()
        {
            EzySlice.Plane cuttingPlane = new EzySlice.Plane();

            // the plane will be set to the same coordinates as the object that this
            // script is attached to
            // NOTE -> Debug Gizmo drawing only works if we pass the transform
            Gizmos.color = Color.red;
            cuttingPlane.Compute(transform);

            // draw gizmos for the plane
            // NOTE -> Debug Gizmo drawing is ONLY available in editor mode. Do NOT try
            // to run this in the final build or you'll get crashes (most likey)
            cuttingPlane.OnDebugDraw();
        }

        private void Awake()
        {
            lastPos = transform.position;
        }

        private void LateUpdate()
        {
            lastPos = Vector3.Lerp(lastPos, transform.position, lerpValue * Time.deltaTime);
        }

        public List<Cuttable> GetCuttables()
        {
            List<Cuttable> cuttables = new List<Cuttable>();
            
            Collider[] colliders = Physics.OverlapBox(lastPos, boxSize, transform.rotation);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Cuttable>(out Cuttable c))
                {
                    cuttables.Add(c);
                }
            }

            return cuttables;
        }
    }
}