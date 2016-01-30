using UnityEngine;
using System.Collections;

public class BlueRedManager : MonoBehaviour {
	public int beginSection2 = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > beginSection2) {
			transform.Rotate (0, 0.5f, 0);
		}
	}
}
