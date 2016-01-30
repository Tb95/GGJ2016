using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Openpause : MonoBehaviour {

    public GameObject Pausa;

	void Start () {

        Pausa.gameObject.SetActive(false);
	
	}

    public void Escpress ()
    {
        Pausa.gameObject.SetActive(true);
        Pausa.transform.GetChild(0).gameObject.SetActive(true);
        Pausa.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        Pausa.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Pausa.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        Time.timeScale = 0;
        Pausa.GetComponent<Pausemenu>().Initialize();
    }

    public void Resume ()
    {
        Pausa.gameObject.SetActive(false);
        Pausa.transform.parent.GetChild(2).gameObject.SetActive(false);
        Pausa.transform.parent.GetChild(3).gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
