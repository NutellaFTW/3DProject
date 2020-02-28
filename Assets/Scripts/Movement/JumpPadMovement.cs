using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadMovement : MonoBehaviour
{
    
    public AudioSource audioSource;
    public AudioClip boinkGoesCoyote;
    private float speed = 1000f;

    public void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == 9)
            return;
        Transform transform1 = transform;
        collider.attachedRigidbody.AddForce(speed * Time.deltaTime * Vector3.up, ForceMode.Impulse);
        audioSource.PlayOneShot(boinkGoesCoyote, 1f);
    }
    
}
