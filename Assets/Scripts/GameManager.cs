using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject player;

	Vector3 StartingPos;
	Quaternion StartingRotate;
	static bool isEnded = false;

	private GUIStyle guiStyle = new GUIStyle();

	public static void RestartStage() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	void Start() {
		StartingPos = GameObject.FindGameObjectWithTag("Start").transform.position;
		StartingRotate = GameObject.FindGameObjectWithTag("Start").transform.rotation;

		GameObject standingCamera = GameObject.FindGameObjectWithTag("MainCamera");
		standingCamera.SetActive(false);
		
		StartingPos = new Vector3(StartingPos.x, StartingPos.y + 2f, StartingPos.z);
		Instantiate(player, StartingPos, StartingRotate);
	}

	void OnGUI() {
		// GUI Stage
		guiStyle.fontSize = 50;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Stage" + (SceneManager.GetActiveScene().buildIndex), guiStyle);
		GUILayout.Space(3);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		// Finish Stage
		if (isEnded) {
			GameObject.Find("FadeOut").GetComponent<FadeOut>().NextSceneFadeAnimation();
			isEnded = false;
		}
	}

	public static void EndGame() {
		isEnded = true;
	}
}
