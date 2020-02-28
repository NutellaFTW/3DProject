using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour {

    public String currentTool;

    public Image[] images = {};

    public int[] sceneLimits = {};

    public TextMeshProUGUI[] text = {};

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public EscapeMenu escapeMenu;

    private String[] tools = {
        "Hole Cutter",
        "Vector Plate",
        "Jump Pad"
    };

    private String lastCurrentTool;

    public void Start() {
        currentTool = tools[0];
        lastCurrentTool = currentTool;
        foreach (TextMeshProUGUI txt in text)
            txt.text = sceneLimits[Array.IndexOf(text, txt)].ToString();
    }

    public void Update() {
        switchTool();
    }

    private void switchTool() {

        if (escapeMenu.activated) {
            currentTool = "Nothing";
            return;
        }
        
        lastCurrentTool = currentTool;

        if (currentTool == "Nothing")
            currentTool = lastCurrentTool;

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        
        if (scroll != 0) {
            
            int toolIndex = Array.IndexOf(tools, currentTool);
            int factor = 0;
                
            if (scroll > 0) {
                if (toolIndex == 0)
                    toolIndex = tools.Length - 1;
                else
                    factor = -1;
            } else {
                if (toolIndex == tools.Length - 1)
                    toolIndex = 0;
                else
                    factor = 1;
            }

            int index = toolIndex + factor;
            
            String newTool = tools[index];

            currentTool = newTool;

            Image selectedImage = images[index];
            
            Color color = selectedImage.color;
            color.a = 1f;

            selectedImage.color = color;

            foreach (Image image in images) {
                if (image != selectedImage) {
                    Color seeThrough = image.color;
                    seeThrough.a = 0.6f;
                    image.color = seeThrough;
                }
            }

        }
        
    }

    public void useTool(int index) {
        sceneLimits[index] -= 1;
        text[index].text = sceneLimits[index].ToString();
        if (index == 0) {
            audioSource.PlayOneShot(audioClips[0], 0.1f);
        }
    }

}
