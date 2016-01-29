using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {

	enum Movement {chase, chaseZigZag, chaseWandering};

	public Movement movement = Movement.chase;
	public int speed;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (movement == Movement.chase)
			chase (player);
		else if (movement == Movement.chaseZigZag)
			chaseZigZag (player);
		else if (movement == Movement.chaseWandering)
			chaseWandering (player);
	}

	void chase(GameObject player) {
		transform.position = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	int deltaXForChaseZigZag = 0;

	void chaseZigZag(GameObject player) {
		if (deltaXForChaseZigZag <= 0.3) {
			deltaXForChaseZigZag = Random.Range (3, 7);
			if (Random.Range (0, 2) == 0) { // Once every two
				deltaXForChaseZigZag = -deltaXForChaseZigZag;
			}
		}
		int tempDeltaX;
		if (deltaXForChaseZigZag > 0) {
			tempDeltaX = speed * Time.deltaTime;
		} else {
			tempDeltaX = (-speed) * Time.deltaTime;
		}
		transform.position = (player.transform.position - transform.position).normalized * speed * Time.deltaTime + tempDeltaX;
		transform.LookAt (player.transform.position);
	}

	int deltaXForChaseWandering = 0;
	int directionForChaseWandering = 0;

	void chaseWandering(GameObject player) {
		if ((deltaXForChaseWandering <= 0.3) && (Random.Range (0, 80) == 0)) {
			deltaXForChaseWandering = Random.Range (3, 7);
			directionForChaseWandering = Random.Range (0, 2);
		}
		if (deltaXForChaseWandering > 0.3) {
			if (directionForChaseWandering == 0) {
				transorm.position += transform.right * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			} else {
				transform.position += transform.right * (-1) * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			}
		} else {
			transform.position = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		}
		transform.LookAt (player.transform.position);
	}
}