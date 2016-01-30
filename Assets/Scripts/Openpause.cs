using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Openpause : MonoBehaviour {

    public GameObject Pausa;

	// Use this for initialization
	void Start () {

        Pausa.gameObject.SetActive(false);
	
	}

    public void Escpress ()
    {
        Pausa.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume ()
    {
        Pausa.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
