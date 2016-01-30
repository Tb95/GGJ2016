using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Spider") {
			other.gameObject.GetComponent<Spider> ().Hit (1, player);
		}

        Destroy(gameObject);
	}
}