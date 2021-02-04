using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VolumetricFogAndMist;

public class Level_01Controller : MonoBehaviour
{
    public float MIN_DISTANCE_TO_PARENTS_FOR_WIN;
    public float MAX_DISTANCE_FROM_PARENTS_FOR_LOSE;

    public GameObject LostChildCharacterGameObject;
    public GameObject MomGameObject;
    public GameObject DadGameObject;

    public float DistanceFromDad;

    public AudioSource bGMlose;
    //public AudioSource bGMwin;

    private AudioSource[] allAudioSources;

    private VolumetricFog fog;
    private float maxDistance;
    private float minDistance;

    public float MaxDensity;
    public float MinDensity;


    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        LostChildCharacterGameObject = GameObject.Find("FirstPersonWalker_Audio");
        MomGameObject = GameObject.Find("Mom");
        DadGameObject = GameObject.Find("Dad");
    }

    public void Start()
    {
        if (bGMlose.isPlaying)
        {
            bGMlose.Stop();
        }

        fog = FindObjectsOfType<VolumetricFog>()[0];

    }

    public void Update()
    {
        CheckDistanceToParents();

        //var distance = (transform.position - Camera.main.transform.position).magnitude;
        var norm = (DistanceFromDad - MIN_DISTANCE_TO_PARENTS_FOR_WIN) / (MAX_DISTANCE_FROM_PARENTS_FOR_LOSE - MIN_DISTANCE_TO_PARENTS_FOR_WIN);
        norm = Mathf.Clamp01(norm);

        fog.density = Mathf.Lerp(MinDensity, MaxDensity, norm);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void TriggerWinCondition()
    {
        StopAllAudio();
        SceneController.LoadScene(1, 1, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
    }

    public void TriggerLoseCondition()
    {
        // Continue to play all the audio from this level into the lose scene
        if (!bGMlose.isPlaying)
        {
            bGMlose.Play();
            SceneController.LoadScene(2, 1, 1f, 0f, 0f, 0f, 1f, 1f, 1f);
        }
    }

    public void CheckDistanceToParents()
    {
        if (DadGameObject == null || MomGameObject == null || LostChildCharacterGameObject == null)
        {
            return;
        }

        DistanceFromDad = (Vector3.Distance(LostChildCharacterGameObject.transform.position, DadGameObject.transform.position));

        if (DistanceFromDad <= MIN_DISTANCE_TO_PARENTS_FOR_WIN)
        {
            TriggerWinCondition();
        }

        if (DistanceFromDad >= MAX_DISTANCE_FROM_PARENTS_FOR_LOSE)
        {
            TriggerLoseCondition();
        }
    }

    void StopAllAudio()
    {
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

}

