using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZinhaMovement : MonoBehaviour {

	public float movePower = 1f;

	Animator animator;

	Vector3 movement;
	int movementFlag = 0;

	void Start () {
		animator = gameObject.GetComponentInChildren<Animator>();

		StartCoroutine("ChangeMovement");
	}

	void FixedUpdate () {
		Move();
	}

	void Move() {
		Vector3 moveVelocity = Vector3.zero;

		if (movementFlag == 1) {
			moveVelocity = Vector3.left;
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (movementFlag == 2) {
			moveVelocity = Vector3.right;
			transform.localScale = new Vector3(-1, 1, 1);
		}

		transform.position += moveVelocity * movePower * Time.deltaTime;
	}

	IEnumerator ChangeMovement() {
		movementFlag = Random.Range(0, 3);

		if (movementFlag == 0) {
			animator.SetBool("isMoving", false);
		}
		else {
			animator.SetBool("isMoving",true);
		}

		yield return new WaitForSeconds(2f);

		StartCoroutine("ChangeMovement");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Zinha") {
			StopCoroutine("ChangeMovement");
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Zinha") {
			animator.SetBool("isMoving", true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Zinha") {
			StartCoroutine("ChangeMovement");
		}
	}
}
