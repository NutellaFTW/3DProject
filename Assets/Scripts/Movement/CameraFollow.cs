using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    
    public void Update() {
        transform.position = player.position;
    }
}
