using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Move : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private BS_HandEvents movementHand;
    [SerializeField] private Transform orientationTransform;

    public float moveSpeed = 2f;
    private float currentGravity;

    // Update is called once per frame
    void Update()
    {
        if (characterController && movementHand)
        {
            if (characterController.isGrounded)
            {
                currentGravity = 0;
            }
            else
            {
                currentGravity += 9.64f * Time.deltaTime;
            }
            
            Vector2 rawInput = movementHand.GetThumbstickValue();
            Vector3 moveInput = orientationTransform.forward * rawInput.y + orientationTransform.right * rawInput.x;
            moveInput.y = 0;
            Vector3 velocity = moveInput + new Vector3(0, -currentGravity, 0);
            
            characterController.Move(moveSpeed * velocity * Time.deltaTime);
        }
    }
}
