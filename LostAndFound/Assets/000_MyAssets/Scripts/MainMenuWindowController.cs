using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainMenuWindowController : MonoBehaviour
{
    public MainSceneScript MainSceneScript;
    public Button StartButton;
    // Start is called before the first frame update
    void Start()
    {
        MainSceneScript = GameObject.Find("MainSceneScript").GetComponent<MainSceneScript>();
        StartButton.onClick.AddListener(StartButtonButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonButtonClick()
    {
        MainSceneScript.Loadlevel();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
