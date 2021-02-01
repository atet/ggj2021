using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource footsteps;
    public AudioSource yell;

    void Update()
    {
        if (Input.GetKeyDown("w") | Input.GetKeyDown("a") | Input.GetKeyDown("s") | Input.GetKeyDown("d"))
        {
            if (!footsteps.isPlaying)
            {
                footsteps.Play();
            }
        }
        if (Input.GetKeyUp("w") | Input.GetKeyUp("a") | Input.GetKeyUp("s") | Input.GetKeyUp("d"))
        {
            if (footsteps.isPlaying)
            {
                footsteps.Stop();
            }
        }
        if (Input.GetKeyDown("space"))
        {
            if (!yell.isPlaying)
            {
                yell.Play();
            }
        }
    }
}
