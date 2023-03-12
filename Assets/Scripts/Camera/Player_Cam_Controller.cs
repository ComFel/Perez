using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    [SerializeField] private float sensibilityX;
    [SerializeField] private float sensibilityY;

    [SerializeField] private Transform playerOrientation;

    private float rotationX;
    private float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibilityY;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        playerOrientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
