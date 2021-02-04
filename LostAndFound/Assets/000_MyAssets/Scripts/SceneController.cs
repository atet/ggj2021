using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
	public Image fader;
	private static SceneController instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
			fader.gameObject.SetActive(false);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static void LoadScene(int index, float duration = 1, float waitTime = 0, float r1 = 0f, float g1 = 0f, float b1 = 0f, float r2 = 0f, float g2 = 0f, float b2 = 0f)
	{
		instance.StartCoroutine(instance.FadeScene(index, duration, waitTime, r1, g1, b1, r2, g2, b2));
	}

	private IEnumerator FadeScene(int index, float duration, float waitTime, float r1 = 0f, float g1 = 0f, float b1 = 0f, float r2 = 0f, float g2 = 0f, float b2 = 0f)
	{
		fader.gameObject.SetActive(true);

		for (float t = 0; t < 1; t += Time.deltaTime / duration)
		{
			fader.color = new Color(r1, g1, b1, Mathf.Lerp(0, 1, t));
			yield return null;
		}

		SceneManager.LoadScene(index);
		//yield return new WaitForSeconds(waitTime);

		for (float t = 0; t < 1; t += Time.deltaTime / waitTime)
		{
			fader.color = new Color(Mathf.Lerp(r1, r2, t), Mathf.Lerp(g1, g2, t), Mathf.Lerp(b1, b2, t), 1f);
			yield return null;
		}

		for (float t = 0; t < 1; t += Time.deltaTime / duration)
		{
			fader.color = new Color(r2, g2, b2, Mathf.Lerp(1, 0, t));
			yield return null;
		}

		fader.gameObject.SetActive(false);
	}
}