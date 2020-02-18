using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour {

    public TextMeshProUGUI gui;
    
    public String currentTool;

    private String[] tools = {
        "Hole Cutter",
        "Vector Plate",
        "Jump Pad"
    };

    public void Start() {
        currentTool = tools[0];
    }

    public void Update() {
        switchTool();
    }

    private void switchTool() {
        
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

            String newTool = tools[toolIndex + factor];

            currentTool = newTool;
            gui.text = currentTool;
            
        }
        
    }

}
