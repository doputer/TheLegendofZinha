using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour {
	public float animTime = 2f;

	private Image fadeImage;

	private float start = 0f;
	private float end = 1f;
	private float time = 0f;

	private bool isPlaying = false;

	void Awake() {
		fadeImage = GetComponent<Image>();
	}

	public void NextSceneFadeAnimation() {
		if (isPlaying) {
			return;
		}
		StartCoroutine("PlaySceneFadeOut");
	}

	public void QuitSceneFadeAnimation() {
		if (isPlaying) {
			return;
		}
		StartCoroutine("QuitSceneFadeOut");
	}

	IEnumerator PlaySceneFadeOut() {
		isPlaying = true;

		Color color = fadeImage.color;
		time = 0f;
		color.a = Mathf.Lerp(start, end, time);

		while (color.a < 1f) {
			time += Time.deltaTime / animTime;
			color.a = Mathf.Lerp(start, end, time);
			fadeImage.color = color;

			yield return null;
		}
		
		isPlaying = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	IEnumerator QuitSceneFadeOut() {
		isPlaying = true;

		Color color = fadeImage.color;
		time = 0f;
		color.a = Mathf.Lerp(start, end, time);

		while (color.a < 1f) {
			time += Time.deltaTime / animTime;
			color.a = Mathf.Lerp(start, end, time);
			fadeImage.color = color;

			yield return null;
		}
		
		isPlaying = false;
		Application.Quit();
	}
}