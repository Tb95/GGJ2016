using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform target; // the object to rotate around
	public Vector3 vec;
	public int speed; // the speed of rotation
	
	void Start() {
		if (target == null) 
		{
			target = this.gameObject.transform;
			Debug.Log ("RotateAround target not specified. Defaulting to parent GameObject");
		}
	}

	// Update is called once per frame
	void Update () {
        if (target == null)
            Destroy(gameObject);

		// RotateAround takes three arguments:
		// first is the Vector to rotate around
		// second is a vector that axis to rotate around
		// third is the degrees to rotate, in this case the speed per second
        else 
		    transform.RotateAround(target.transform.position, vec, speed * Time.deltaTime);
	}
}
