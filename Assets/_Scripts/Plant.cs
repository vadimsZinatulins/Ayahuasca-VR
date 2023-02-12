using UnityEngine;

namespace DefaultNamespace
{
    public class Plant : BS_Grabbable
    {
        public GameObject plantDroppable;
        public override void OnPlayerGrip(BS_HandEvents hand, bool InValue)
        {
            if (InValue)
            {
                Debug.Log("Spawned Plant", this);
                var GOspawned = Instantiate(plantDroppable, hand.transform.position, hand.transform.rotation);
                Rigidbody rb = GOspawned.GetComponent<Rigidbody>();
                hand.OverrideHandGrabbable(rb);
            }
        }
    }
}