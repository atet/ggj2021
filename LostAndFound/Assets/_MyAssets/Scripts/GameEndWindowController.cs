using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameEndWindowController : MonoBehaviour
{
    public Button ReplayButton;
    // Start is called before the first frame update
    void Start()
    {
        ReplayButton.onClick.AddListener(ReplayButtonButtonClick);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplayButtonButtonClick()
    {
        Debug.LogError("Clicked On Replay Button");
    }
}
