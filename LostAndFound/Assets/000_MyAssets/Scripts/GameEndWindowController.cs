using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameEndWindowController : MonoBehaviour
{
    public MainSceneScript MainSceneScript;
    public Button ReplayButton;

    void Start()
    {
        MainSceneScript = GameObject.Find("MainSceneScript").GetComponent<MainSceneScript>();
        ReplayButton.onClick.AddListener(ReplayButtonButtonClick);
    }

    void Update()
    {
        
    }

    public void ReplayButtonButtonClick()
    {
        Debug.Log("Clicked On Replay Button");
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
