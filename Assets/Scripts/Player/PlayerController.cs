using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Public-Varibles")]
    public CharacterController myController;
    public bool rotMov;
    public float speed = 5f;
    public float rotSpeed = 5f;

    private float gravity = 9.8f;
    private float rotation;
    private Vector3 myDirection;

    void Start()
    {
        myController = gameObject.GetComponent<CharacterController>();        
    }

    void Update()
    {
        if (rotMov)
        {
            myDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Vertical"),0, Input.GetAxis("Vertical")) * speed);
            rotation = Input.GetAxis("Horizontal") * rotSpeed;
            myDirection -= new Vector3(0, gravity * Time.deltaTime, 0);
            myController.transform.Rotate(new Vector3(0, rotation, 0));
            myController.Move(myDirection * Time.deltaTime);
        }
        else
        {
            myDirection.x = Input.GetAxis("Horizontal") * speed;
            myDirection.z = Input.GetAxis("Vertical") * speed;

            myDirection.y -= gravity * Time.deltaTime;

            myController.Move(myDirection * Time.deltaTime);
        }
    }
}
