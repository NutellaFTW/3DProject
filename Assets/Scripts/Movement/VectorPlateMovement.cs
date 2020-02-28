using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPlateMovement : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip vroomGoesCoyote;
    private float speed = 1000f;

    public void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == 9)
            return;
        Transform transform1 = transform;
        collider.attachedRigidbody.AddForce(speed * Time.deltaTime * (transform1.rotation * transform1.forward) * -1, ForceMode.Impulse);
        audioSource.PlayOneShot(vroomGoesCoyote, 0.1f);
    }
    
}
