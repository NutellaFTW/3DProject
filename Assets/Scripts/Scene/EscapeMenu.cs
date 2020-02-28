using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour {

    public Animator animator;
    public Image image;
    public GameObject escapeMenu;

    public bool activated = false;

    public void MainMenu() {
        StartCoroutine(Go("MainMenu"));
    }
    
    public void RestartButton() {
        StartCoroutine(Go(SceneManager.GetActiveScene().name));
    }

    private IEnumerator Go(String scene) {
        escapeMenu.SetActive(false);
        Time.timeScale = 1;
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => image.color.a == 1);
        SceneManager.LoadScene(scene);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            activated = !activated;
            escapeMenu.SetActive(activated);
            Time.timeScale = activated ? 0 : 1;
            Cursor.lockState = activated ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = activated;
        }
    }
    
}
