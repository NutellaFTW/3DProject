using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadMovement : MonoBehaviour
{
    private float speed = 1000f;

    public void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player"))
            collider.GetComponent<CharacterController>().Move(speed * Time.deltaTime * transform.forward);
        else {
            Transform transform1 = transform;
            collider.attachedRigidbody.AddForce(speed * Time.deltaTime * Vector3.up, ForceMode.Impulse);
        }
    }
    
}
