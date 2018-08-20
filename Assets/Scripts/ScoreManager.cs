using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	static int score = 0;

	private GUIStyle guiStyle = new GUIStyle();

	public static void setScore(int value) {
		score += value;
	}

	public static int getScore() {
		return score;
	}

	void OnGUI() {
		guiStyle.fontSize = 50;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label("Score: " + score.ToString(), guiStyle);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
