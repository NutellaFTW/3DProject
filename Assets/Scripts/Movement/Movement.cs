using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -15f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    public Vector3 velocity;
    
    private bool isGrounded;
    

    public void Update() {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y <0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Transform playerTransform = transform;
        Vector3 move = playerTransform.right * x + playerTransform.forward * z;

        controller.Move(speed * Time.deltaTime * move);

        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
    }
    
}
