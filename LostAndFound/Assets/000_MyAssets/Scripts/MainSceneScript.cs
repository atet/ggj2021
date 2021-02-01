using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public float MIN_DISTANCE_TO_PARENTS_FOR_WIN = 1;
    public float MAX_DISTANCE_FROM_PARENTS_FOR_LOSE = 75;

    public MainMenuWindowController MainMenuWindowController;
    public GameEndWindowController GameWinWindowController;
    public GameEndWindowController GameLoseWindowController;

    public GameObject LostChildCharacterGameObject;
    public GameObject MomGameObject;
    public GameObject DadGameObject;

    public float DistanceFromDad;

    public AudioSource bGMwin;
    public AudioSource bGMlose;

    private AudioSource[] allAudioSources;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        MainMenuWindowController.Hide();
        GameWinWindowController.Hide();
        GameLoseWindowController.Hide();
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    public void Start()
    {
        MainMenuWindowController.Show();
    }

    public void Update()
    {
        CheckDistanceToParents();
    }

    public void OnLevelWasLoaded(int level)
    {
        MakeReferencesToCharacters();
    }

    private void MakeReferencesToCharacters()
    {
        LostChildCharacterGameObject = GameObject.Find("FirstPersonWalker_Audio");
        MomGameObject = GameObject.Find("Mom");
        DadGameObject = GameObject.Find("Dad");
    }

    public void Loadlevel()
    {
        if (bGMlose.isPlaying)
        {
            bGMlose.Stop();
        }
        if (bGMwin.isPlaying)
        {
            bGMwin.Stop();
        }
        MainMenuWindowController.Hide();
        GameWinWindowController.Hide();
        GameLoseWindowController.Hide();
        SceneManager.LoadScene("Prototype01");
    }

    public void TriggerWinCondition()
    {
        StopAllAudio();
        if (!bGMwin.isPlaying)
        {
            bGMwin.Play();
        }
        MainMenuWindowController.Hide();
        GameWinWindowController.Show();
        GameLoseWindowController.Hide();
    }

    public void TriggerLoseCondition()
    {
        if (!bGMlose.isPlaying)
        {
            bGMlose.Play();
        }
        MainMenuWindowController.Hide();
        GameWinWindowController.Hide();
        GameLoseWindowController.Show();
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

