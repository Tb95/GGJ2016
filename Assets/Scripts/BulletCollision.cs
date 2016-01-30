using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Spider") {
<<<<<<< HEAD
			other.gameObject.GetComponent<Spider> ().Hit (1);
			Destroy (gameObject);
=======
			other.gameObject.GetComponent<Spider> ().Hit (1, player);
>>>>>>> d360d0edc6b4165b4b8a9417eb25c80b495019af
		}

        Destroy(gameObject);
	}
}