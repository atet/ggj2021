using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdColorRandom : MonoBehaviour
{
 
// Start is called before the first frame update
    void Start()
    {
        var randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
            sprites[i].color = randomColor;
        }


    }





}
