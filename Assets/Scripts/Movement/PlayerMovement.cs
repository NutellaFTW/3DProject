using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float sensitivity = 50f;

    public float moveSpeed = 4500f;
    public float maxSpeed = 20f;
    public float counterMovement = 0.175f;
    
    private Camera playerCamera;

    private float desiredX, rotationX;

    private float x, y;
    private bool jumping;
    
    private float threshold = 0.01f;

    private Rigidbody rigidBody;

    public Transform orientation;

    public void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = Camera.main;
        rigidBody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate() {
        
        rigidBody.AddForce(Time.deltaTime * 10 * Vector3.down);

        Vector2 magnitude = GetVelocityRelativeToRotation();

        float magnitudeX = magnitude.x;
        float magnitudeY = magnitude.y;
        
        ContrastMovement(x, y, magnitude);

        if (x > 0 && magnitudeX > maxSpeed) 
            x = 0;
        
        if (x < 0 && magnitudeX < -maxSpeed) 
            x = 0;
        
        if (y > 0 && magnitudeY > maxSpeed) 
            y = 0;
        
        if (y < 0 && magnitudeY < -maxSpeed) 
            y = 0;
        
        rigidBody.AddForce(x * moveSpeed * Time.deltaTime * orientation.transform.right);
        rigidBody.AddForce(y * moveSpeed * Time.deltaTime * orientation.transform.forward);

    }

    public void Update() {

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        
        // START LOOK
        
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;

        desiredX = rotation.y + mouseX;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, desiredX, 0);
        
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        
        // END LOOK        

    }

    private Vector2 GetVelocityRelativeToRotation() {

        float lookAngle = orientation.transform.eulerAngles.y;
        
        Vector3 velocity = rigidBody.velocity;
        
        float moveAngle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;

        float delta = Mathf.DeltaAngle(lookAngle, moveAngle);
        float theta = 90 - delta;

        float magnitude = rigidBody.velocity.magnitude;
        float magnitudeX = magnitude * Mathf.Cos(theta * Mathf.Deg2Rad);
        float magnitudeY = magnitude * Mathf.Cos(delta * Mathf.Deg2Rad);

        return new Vector2(magnitudeX, magnitudeY);
        
    }

    private void ContrastMovement(float x, float y, Vector2 magnitude) {
        
        if (Math.Abs(magnitude.x) > threshold && Math.Abs(x) < 0.05f || (magnitude.x < -threshold && x > 0) || (magnitude.x > threshold && x < 0)) 
            rigidBody.AddForce(moveSpeed * Time.deltaTime * -magnitude.x * counterMovement * orientation.transform.right);

        if (Math.Abs(magnitude.y) > threshold && Math.Abs(y) < 0.05f || (magnitude.y < -threshold && y > 0) || (magnitude.y > threshold && y < 0)) 
            rigidBody.AddForce(moveSpeed * Time.deltaTime * -magnitude.y * counterMovement * orientation.transform.forward);
        
        if (Mathf.Sqrt((Mathf.Pow(rigidBody.velocity.x, 2) + Mathf.Pow(rigidBody.velocity.z, 2))) > maxSpeed) {
            Vector3 velocity = rigidBody.velocity;
            float fallspeed = velocity.y;
            Vector3 normalized = velocity.normalized * maxSpeed;
            rigidBody.velocity = new Vector3(normalized.x, fallspeed, normalized.z);
        }

    }
    
}
