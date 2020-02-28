using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLOpen : MonoBehaviour
{

    public void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenItch() {
        Application.OpenURL("https://microbox.itch.io/holeberg");
    }
    
}
