using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Mainmenu : MonoBehaviour {
    public Canvas Exitmenu;
    public Canvas Startmenu;
    public Canvas Optionmenu;
    public Button Startbuton;
    public Button Optionbuton;
    public Button Exitbutton;

	// Use this for initialization
	void Start () {
        Exitmenu = Exitmenu.GetComponent<Canvas> ();
        Startmenu = Startmenu.GetComponent<Canvas> ();
        Optionmenu = Optionmenu.GetComponent<Canvas> ();
        Startbuton = Startbuton.GetComponent<Button> ();
        Optionbuton = Optionbuton.GetComponent<Button> ();
        Exitbutton = Exitbutton.GetComponent<Button>(); 
        Exitmenu.gameObject.SetActive (false);
        Startmenu.gameObject.SetActive (false);
        Optionmenu.gameObject.SetActive (false);
	}
    
    public void Exitpress()
    {
        Exitmenu.gameObject.SetActive (true);
        Startbuton.gameObject.SetActive (false);
        Optionbuton.gameObject.SetActive (false);
        Exitbutton.gameObject.SetActive (false);
    }

    public void Startpress()
    {
        Startmenu.gameObject.SetActive(true);
        Startbuton.gameObject.SetActive(false);
        Optionbuton.gameObject.SetActive(false);
        Exitbutton.gameObject.SetActive(false);
    }

    public void Optionpress()
    {
        Optionmenu.gameObject.SetActive(true);
        Startbuton.gameObject.SetActive(false);
        Optionbuton.gameObject.SetActive(false);
        Exitbutton.gameObject.SetActive(false);
    }

    public void Nopress()
    {
        Exitmenu.gameObject.SetActive(false);
        Startbuton.gameObject.SetActive(true);
        Optionbuton.gameObject.SetActive(true);
        Exitbutton.gameObject.SetActive(true);
    }

    public void Yespress ()
    {
        Application.Quit();
    }

    public void Backpress ()
    {
        Startmenu.gameObject.SetActive (false);
        Optionmenu.gameObject.SetActive(false);
        Startbuton.gameObject.SetActive(true);
        Optionbuton.gameObject.SetActive(true);
        Exitbutton.gameObject.SetActive(true);
    }

    public void playerone () 
    {
        SceneManager.LoadScene(1);
    }

    public void playertwo ()
    {
        SceneManager.LoadScene(2);
    }


	// Update is called once per frame
	void Update () {
	
	}
}
