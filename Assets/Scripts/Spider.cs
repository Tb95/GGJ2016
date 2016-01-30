using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {

	public enum Movement {chase, chaseZigZag, chaseWandering};

	public Movement movement = Movement.chase;
	public int speed;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (transform.position, player.transform.position);
		// If dist < 0.3 : do nothing
		// If 0.3 < dist < 2 : just chase
		// If 2 < dist : perform movement
		if (dist >= 0.3 && dist <= 2) {
			chase (player);
		} else if (dist > 2) {
			if (movement == Movement.chase)
				chase (player);
			else if (movement == Movement.chaseZigZag)
				chaseZigZag (player);
			else if (movement == Movement.chaseWandering)
				chaseWandering (player);
		}
	}

	void chase(GameObject player) {
		transform.position += (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	float deltaXForChaseZigZag = 0;

	void chaseZigZag(GameObject player) {
		if ((deltaXForChaseZigZag <= 0.3) && (deltaXForChaseZigZag >= -0.3)) {
			deltaXForChaseZigZag = (float)(Random.Range (3, 7));
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

	void chaseWandering(GameObject player) {
		if ((deltaXForChaseWandering <= 0.3) && (Random.Range (0, 60) == 0)) {
			deltaXForChaseWandering = (float)(Random.Range (3, 7));
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