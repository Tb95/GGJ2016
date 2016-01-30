using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pausemenu : MonoBehaviour {

    public Canvas Gotomainm;
    public Canvas Optionmenu;
    public Canvas Buttoncanvas;
    public Button Resumebutton;
    public Button Optionbutton;
    public Button Mainbutton;

	// Use this for initialization
	void Start () {

        Gotomainm = Gotomainm.GetComponent<Canvas>();
        Buttoncanvas = Buttoncanvas.GetComponent<Canvas>();
        Optionmenu = Optionmenu.GetComponent<Canvas>();
        Resumebutton = Resumebutton.GetComponent<Button>();
        Optionbutton = Optionbutton.GetComponent<Button>();
        Mainbutton = Mainbutton.GetComponent<Button>();

        Gotomainm.gameObject.SetActive(false);
        Optionmenu.gameObject.SetActive(false);
	
	}
	
    public void Optionpress ()
    {
        Optionmenu.gameObject.SetActive(true);
        Buttoncanvas.gameObject.SetActive(false);
    }

    public void Mainpress ()
    {
        Gotomainm.gameObject.SetActive(true);
        Resumebutton.gameObject.SetActive(false);
        Optionbutton.gameObject.SetActive(false);
        Mainbutton.gameObject.SetActive(false);
    }

    public void Yespress ()
    {
        Application.LoadLevel(0);
    }

    public void backpress ()
    {
        Gotomainm.gameObject.SetActive(false);
        Optionmenu.gameObject.SetActive(false);
        Resumebutton.gameObject.SetActive(true);
        Optionbutton.gameObject.SetActive(true);
        Mainbutton.gameObject.SetActive(true);
        Buttoncanvas.gameObject.SetActive(true);
    }

    

	// Update is called once per frame
	void Update () {
	
	}
}
