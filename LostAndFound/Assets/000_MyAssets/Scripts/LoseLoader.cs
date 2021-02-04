using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLoader : MonoBehaviour
{
	private AudioSource[] allAudioSources;

	private void Awake()
	{
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
	}

	private void Start()
	{
		StopAllAudio();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}

	public void LoadSceneWhite(int i)
	{
		//SceneManager.LoadScene(i);
		SceneController.LoadScene(i, 1, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
	}

	public void LoadSceneBlack(int i)
	{
		//SceneManager.LoadScene(i);
		SceneController.LoadScene(i, 1, 1f, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public void LoadSceneWhiteBlack(int i)
	{
		//SceneManager.LoadScene(i);
		SceneController.LoadScene(i, 1, 1f, 1f, 1f, 1f, 0f, 0f, 0f);
	}

	public void LoadSceneBlackWhite(int i)
	{
		//SceneManager.LoadScene(i);
		SceneController.LoadScene(i, 1, 1f, 0f, 0f, 0f, 1f, 1f, 1f);
	}

	void StopAllAudio()
	{
		foreach (AudioSource audioS in allAudioSources)
		{
			audioS.Stop();
		}
	}
}