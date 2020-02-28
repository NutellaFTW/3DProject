using UnityEngine;
using System.Collections;

public class MusicSingleton : MonoBehaviour 
{
    public static MusicSingleton instance;

    public void Awake() {
        if (instance)
            DestroyImmediate(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}