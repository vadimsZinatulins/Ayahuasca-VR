using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Player : MonoBehaviour
{
    public static BS_Player Instance;
    public Transform playerCameraPosition;
    public Transform playerBodyPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
}
