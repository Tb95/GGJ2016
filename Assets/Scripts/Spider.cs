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

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		buttonsManager = new ButtonsManager ();
		comboList = buttonsManager.getRandomCombo(comboLength, player.GetComponent<InputManager>().getRandomSide());
		player.GetComponent<InputManager> ().possibleSpiderCombos.Add(new SpiderCombo(comboList, this));
		for (int i = 0; i < comboList.Count; i++) {
			Debug.Log (comboList [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
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
}