using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRenderQueue : MonoBehaviour
{
    public int renderQueuePosition = 100;

    void Start() {
        GetComponent<Material>().renderQueue = renderQueuePosition;
    }
    
}
