using UnityEngine;
using System.Collections;

public class PlayBackgroud : MonoBehaviour {

	public AudioClip clip1;
	public AudioClip clip2;
	bool playing1 = false;
	AudioSource aS;

	// Use this for initialization
	void Start () {
		aS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!playing1 && !aS.isPlaying) {
			playing1 = true;
			aS.clip = clip1;
			aS.Play();
			Debug.Log ("PLAY1");
		} else if (playing1 && !aS.isPlaying){
			aS.clip = clip2;
			aS.Play();
			Debug.Log ("PLAY2");
		}
	}
}
