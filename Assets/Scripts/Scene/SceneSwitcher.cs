using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour {

    public Animator animator;
    public Image image;

    public void StartButton() {
        StartCoroutine(Go());
    }

    private IEnumerator Go() {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => image.color.a == 1);
        SceneManager.LoadScene("Level1");
    }

    public void Exit() {
        Application.Quit();
    }
    
}
