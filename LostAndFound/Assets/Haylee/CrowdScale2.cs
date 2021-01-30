using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScale2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
transform.localScale = new Vector3 (Random.Range(0.5f,1.2f),Random.Range(0.5f,1.5f),transform.localScale.z);         
    }
}
