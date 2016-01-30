using UnityEngine;
using System.Collections;

public class BlueRedManager : MonoBehaviour {
	public int beginSection2 = 10;
	public int beginSection3 = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > beginSection2 && Time.time <= beginSection3) {
			transform.Rotate (0, 0.5f, 0);
		} else if (Time.time > beginSection3) {
			transform.Rotate (0,  Mathf.Abs (Mathf.Sin (Time.time)), 0);
		}
	}
}
