using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Flag"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Update() {
        if (transform.position.y <= -10)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
