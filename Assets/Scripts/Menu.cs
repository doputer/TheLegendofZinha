using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	public void PlayGame() {
		GameObject.Find("FadeOut").GetComponent<FadeOut>().NextSceneFadeAnimation();
	}

	public void QuitGame() {
		GameObject.Find("FadeOut").GetComponent<FadeOut>().QuitSceneFadeAnimation();
	}
}