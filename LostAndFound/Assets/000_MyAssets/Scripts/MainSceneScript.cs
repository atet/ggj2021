using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public float MIN_DISTANCE_TO_PARENTS_FOR_WIN = 10;
    public float MAX_DISTANCE_FROM_PARENTS_FOR_LOSE = 50;

    public MainMenuWindowController MainMenuWindowController;
    public GameEndWindowController GameWinWindowController;
    public GameEndWindowController GameLoseWindowController;

    public GameObject LostChildCharacterGameObject;
    public GameObject MomGameObject;
    public GameObject DadGameObject;

    public float DistanceFromDad;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        MainMenuWindowController.Hide();
        GameWinWindowController.Hide();
        GameLoseWindowController.Hide();
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
        MainMenuWindowController.Hide();
        GameWinWindowController.Hide();
        GameLoseWindowController.Hide();
        SceneManager.LoadScene("Prototype01");
    }

    public void TriggerWinCondition()
    {
        MainMenuWindowController.Hide();
        GameWinWindowController.Show();
        GameLoseWindowController.Hide();
    }

    public void TriggerLoseCondition()
    {
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
}

