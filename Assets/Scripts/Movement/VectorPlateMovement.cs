using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPlateMovement : MonoBehaviour {

    private float speed = 1000f;

    public void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player"))
            collider.GetComponent<CharacterController>().Move(speed * Time.deltaTime * transform.forward);
        else {
            Transform transform1 = transform;
            collider.attachedRigidbody.AddForce(speed * Time.deltaTime * (transform1.rotation * transform1.forward) * -1, ForceMode.Impulse);
        }
    }
    
}
