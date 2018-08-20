using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public float movePower = 1.5f;
	public float jumpPower = 1.5f;

	public int maxHealth = 3;

	Rigidbody2D rigid;
	Animator animator;
	SpriteRenderer spriteRenderer;
	GameObject character;
	GameObject key;
	GameObject prison;

	Vector3 movement;
	int jumpCount = 0;
	bool doubleJump = false;
	bool isJumping = false;
	bool onJumping = false;

	int health = 3;
	bool isDie = false;
	bool isUnBeatTime = false;

	bool isKey = false;
	bool isPrison = true;

	private GUIStyle guiStyle = new GUIStyle();

	void Start() {
		rigid = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponentInChildren<Animator>();
		character = GameObject.Find("Character");
		spriteRenderer = character.GetComponentInChildren<SpriteRenderer>();
		key = GameObject.Find("Key");
		prison = GameObject.Find("Prison");

		health = maxHealth;
	}
	
	void Update() {
		if (health == 0) {
			if(!isDie) {
				Die();
			}
			return;
		}

		// Moving
		if (Input.GetAxisRaw("Horizontal") == 0 ) {
			animator.SetBool("isWalking", false);
		}
		else if (Input.GetAxisRaw("Horizontal") < 0) {
			animator.SetInteger("Direction", -1);
			animator.SetBool("isWalking", true);
		}
		else if (Input.GetAxisRaw("Horizontal") > 0) {
			animator.SetInteger("Direction", 1);
			animator.SetBool("isWalking", true);
		}

		// Jumping
		if ((Input.GetButtonDown("Jump") && !onJumping) || Input.GetButtonDown("Jump") && doubleJump) {
			isJumping = true;
			onJumping = true;

			jumpCount++;
			if (jumpCount == 2) {
				doubleJump = false;
				jumpCount = 0;
			}
		}
	}

	void FixedUpdate() {
		if (health == 0) {
			return;
		}

		Move();
		Jump();
	}

	void Move() {
		Vector3 moveVelocity = Vector3.zero;

		if (Input.GetAxisRaw("Horizontal") < 0) {
			moveVelocity = Vector3.left;
		}
		else if (Input.GetAxisRaw("Horizontal") > 0) {
			moveVelocity = Vector3.right;
		}

		transform.position += moveVelocity * movePower * Time.deltaTime;
	}

	void Jump() {
		if (isJumping) {	
			rigid.velocity = Vector2.zero;

			Vector2 jumpVelocity = new Vector2(0, jumpPower);
			rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

			isJumping = false;
		}
	}

	void Die() {
		isDie = true;

		rigid.velocity = Vector2.zero;

		animator.Play("Fall");

		BoxCollider2D[] colls = gameObject.GetComponents<BoxCollider2D>();
		colls[0].enabled = false;
		colls[1].enabled = false;

		Vector2 dieVelocity = new Vector2(0, 10f);
		rigid.AddForce(dieVelocity, ForceMode2D.Impulse);

		Invoke("RestartStage", 2f);
	}

	void RestartStage() {
		GameManager.RestartStage();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == 0 || other.gameObject.layer == 9) {
			onJumping = false;
		}

		if (other.gameObject.layer == 9) {
			BlockStatus block = other.gameObject.GetComponent<BlockStatus>();

			switch(block.type) {
				case "Jumper":
					Vector2 jumperVelocity = new Vector2(0, block.value);
					rigid.AddForce(jumperVelocity, ForceMode2D.Impulse);
					break;
				case "DJumper":
					jumpCount = 0;
					doubleJump = true;
					break;
				case "PortalEnter":
					Vector3 anotherPortalPos = block.portal.transform.position;
					Vector3 warpPos = new Vector3(anotherPortalPos.x, anotherPortalPos.y + 2f, anotherPortalPos.z);
					transform.position = warpPos;
					break;
			}
		}

		if (other.gameObject.tag == "Creature" && !other.isTrigger && rigid.velocity.y < -4f) {
			CreatureMovement creature = other.gameObject.GetComponent<CreatureMovement>();
			creature.Die();

			Vector2 killVelocity = new Vector2(0, 14f);
			rigid.AddForce(killVelocity, ForceMode2D.Impulse);

			ScoreManager.setScore(creature.score);
		}
		else if (other.gameObject.tag == "Creature" && !other.isTrigger && !(rigid.velocity.y < -4f) && !isUnBeatTime) {
			Vector2 attackedVelocity = Vector2.zero;

			if (other.gameObject.transform.position.x > transform.position.x) {
				attackedVelocity = new Vector2 (-2f, 7f);
			}
			else {
				attackedVelocity = new Vector2 (2f, 7f);
			}
			rigid.AddForce(attackedVelocity, ForceMode2D.Impulse);

			health--;
			if (health > 0) {
				isUnBeatTime = true;
				StartCoroutine("UnBeatTime");
			}
		}

		if (other.gameObject.tag == "Fall") {
			health = 0;
		}

		if (other.gameObject.tag == "Coin") {
			BlockStatus coin = other.gameObject.GetComponent<BlockStatus>();
			ScoreManager.setScore((int)coin.value);

			Destroy(other.gameObject, 0f);
		}

		if (other.gameObject.tag == "End") {
			GameManager.EndGame();
		}

		if (other.gameObject.tag == "Key") {
			isKey = true;
			Destroy(key);
		}

		if (other.gameObject.tag == "Prison") {
			if (isKey) {
				isPrison = false;
				Destroy(prison);
			}
		}

		if (other.gameObject.tag == "Zinha") {
			if (isKey) {
				if (!isPrison) {
					GameManager.EndGame();
				}
			}
		}
	}

	IEnumerator UnBeatTime() {
		int countTime = 0;

		while (countTime < 10) {
			if (countTime % 2 == 0) {
				spriteRenderer.color = new Color32(255, 255, 255, 90);
			}
			else {
				spriteRenderer.color = new Color32(255, 255, 255, 180);
			}

			yield return new WaitForSeconds(0.2f);

			countTime++;
		}

		spriteRenderer.color = new Color32(255, 255, 255, 255);

		isUnBeatTime = false;

		yield return null;
	}

	void OnGUI() {
		// Health
		guiStyle.fontSize = 30;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		GUILayout.Space(12);

		string heart = "";
		for (int i = 0; i < health; i++) {
			heart += "♥ ";
		}
		GUILayout.Label(heart, guiStyle);

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
