using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Camera : MonoBehaviour
{
    [SerializeField] private Transform cameraPlayerPosition;

    private void Update()
    {
        transform.position = cameraPlayerPosition.position;
    }
}
