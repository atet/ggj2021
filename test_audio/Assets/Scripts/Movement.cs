using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioSource footsteps;
    public AudioSource yell;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
        {
            transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("left"))
        {
            transform.position += new Vector3(-1 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKeyDown("right") | Input.GetKeyDown("left"))
        {
            if (!footsteps.isPlaying)
            {
                footsteps.Play();
            }
        }
        if (Input.GetKeyUp("right") | Input.GetKeyUp("left"))
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
