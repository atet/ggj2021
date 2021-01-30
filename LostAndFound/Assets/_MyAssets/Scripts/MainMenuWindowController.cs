using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenuWindowController : MonoBehaviour
{
    public Button StartButton;
    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartButtonButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonButtonClick()
    {
        Debug.LogError("Clicked On Start Button");
    }
}
