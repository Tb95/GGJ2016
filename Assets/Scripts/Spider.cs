using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spider : MonoBehaviour {

	public enum Movement {chase, chaseZigZag, chaseArchs};

	public Movement movement = Movement.chase;
	// BASIC
	public int speed = 4;
	public float doNothingTriggerDistance = 0.3f;
	public float chaseTriggerDistance = 2.0f;
	public bool isDown = false;
	public InputManager.Side side;
	public int health = 3;
	public float downTime = 2.0f;
	// ZIG ZAG
	public int minZigZagLength = 3;
	public int maxZigZagLength = 7;
	// ARCHS
	public int minArchLength = 3;
	public int maxArchLength = 7;
	public int archProbability = 40;
	public GameObject player;
	// COMBO
	public int comboLength = 3;
	ButtonsManager buttonsManager;
	public List<ButtonsManager.Button> comboList;
	// RADIUS FOR ATTACK
	public float radiusForAttack = 4.0f;
	// AUDIO CLIPS
	public AudioClip flippinSpider;
	public AudioClip spiderCatch;
	public AudioClip spiderKill;
	// BAD THINGS
	SpiderCombo spiderCombo;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		side = player.GetComponent<InputManager> ().getRandomSide ();
		buttonsManager = new ButtonsManager ();
		comboList = buttonsManager.getRandomCombo(comboLength, side);
		spiderCombo = new SpiderCombo (comboList, this);
		player.GetComponent<InputManager> ().possibleSpiderCombos.Add(spiderCombo);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDown) {
			float dist = Vector3.Distance (transform.position, player.transform.position);
			// If dist < 0.3 : do nothing
			// If 0.3 < dist < 2 : just chase
			// If 2 < dist : perform movement
			if (dist >= doNothingTriggerDistance && dist <= chaseTriggerDistance) {
				chase (player);
			} else if (dist > chaseTriggerDistance) {
				if (movement == Movement.chase)
					chase (player);
				else if (movement == Movement.chaseZigZag)
					chaseZigZag (player);
				else if (movement == Movement.chaseArchs)
					chaseArchs (player);
			}
		}

		// MANAGE IS DOWN
		else {
			// 1 - Mostra combo
			// 2 - Dopo tot tempo torna not down

			// 2
			if ((Time.time - timeOfGettingDown) >= downTime) {
				isDown = false;

				// Rotate by 180 degrees
				gameObject.transform.RotateAround(gameObject.transform.forward, 180.0f);

				// Reenable trigger that causes the player to lose health
				gameObject.GetComponent<CapsuleCollider>().enabled = true;
			}
		}
	}

	void chase(GameObject player) {
		transform.position += (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	float deltaXForChaseZigZag = 0;

	void chaseZigZag(GameObject player) {
		if ((deltaXForChaseZigZag <= 0.3) && (deltaXForChaseZigZag >= -0.3)) {
			deltaXForChaseZigZag = (float)(Random.Range (minZigZagLength, maxZigZagLength + 1));
			if (Random.Range (0, 2) == 0) { // Once every two
				deltaXForChaseZigZag = -deltaXForChaseZigZag;
			}
		}
		float tempDeltaX;
		if (deltaXForChaseZigZag > 0) {
			tempDeltaX = speed * Time.deltaTime;
		} else {
			tempDeltaX = (-speed) * Time.deltaTime;
		}
		deltaXForChaseZigZag -= tempDeltaX;
		transform.position += ((player.transform.position - transform.position).normalized + (transform.right * tempDeltaX).normalized).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	float deltaXForChaseWandering = 0;
	int directionForChaseWandering = 0;

	void chaseArchs(GameObject player) {
		if ((deltaXForChaseWandering <= 0.3) && (Random.Range (0, 100 - archProbability) == 0)) {
			deltaXForChaseWandering = (float)(Random.Range (minArchLength, maxArchLength + 1));
			directionForChaseWandering = Random.Range (0, 2);
		}
		if (deltaXForChaseWandering > 0.3) {
			if (directionForChaseWandering == 0) {
				transform.position += transform.right * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			} else {
				transform.position += transform.right * (-1) * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			}
		} else {
			transform.position += (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		}
		transform.LookAt (player.transform.position);
	}

	float lastAttackTime = 0;
	public float attackEveryTotSeconds = 0.5f;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && (Time.time - lastAttackTime) > attackEveryTotSeconds) {
			other.gameObject.GetComponent<Player> ().Hit (1);
			lastAttackTime = Time.time;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player" && (Time.time - lastAttackTime) > attackEveryTotSeconds) {
			other.gameObject.GetComponent<Player> ().Hit (1);

			// Play sound
			gameObject.GetComponent<AudioSource>().clip = spiderCatch;
			gameObject.GetComponent<AudioSource> ().Play ();

			lastAttackTime = Time.time;
		}
	}

	float timeOfGettingDown = 0;
	public void Hit(int damage) {
		health -= damage;
		if (health <= 0) {
			player.GetComponent<InputManager> ().possibleSpiderCombos.Remove (spiderCombo);

			// Play sound
			player.GetComponent<AudioSource>().clip = spiderKill;
			player.GetComponent<AudioSource> ().Play ();

			Destroy (gameObject);
		} else {
			isDown = true;
			timeOfGettingDown = Time.time;
			Debug.Log (comboList [0] + "");

			// Rotate by 180 degrees
			gameObject.transform.RotateAround(gameObject.transform.forward, 180.0f);

			// Play sound
			gameObject.GetComponent<AudioSource>().clip = flippinSpider;
			gameObject.GetComponent<AudioSource> ().Play ();

			// Deactivate trigger that cause the player to lose health
			gameObject.GetComponent<CapsuleCollider>().enabled = false;
		}
	}
}