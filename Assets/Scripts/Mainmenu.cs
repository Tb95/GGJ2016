using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Mainmenu : MonoBehaviour {
    public Canvas Exitmenu;
    public Canvas Startmenu;
    public Canvas Optionmenu;
    public Button Startbuton;
    public Button Optionbuton;
    public Button Exitbutton;
    public AudioMixer mainMixer;


    Selectable _buttonOn;
    Selectable ButtonOn
    {
        get { return _buttonOn; }
        set
        {
            if (_buttonOn != value)
            {
                _buttonOn = value;
                value.Select();
            }
        }
    }
    List<Selectable> buttons;
    int currentButtonIndex;
    bool dPadPressed;

	void Start () {
        Exitmenu.gameObject.SetActive (false);
        Startmenu.gameObject.SetActive (false);
        Optionmenu.gameObject.SetActive (false);

        float volume;
        mainMixer.GetFloat("musicVol", out volume);
        Optionmenu.transform.GetChild(0).GetComponent<Slider>().value = volume;
        mainMixer.GetFloat("sfxVol", out volume);
        Optionmenu.transform.GetChild(1).GetComponent<Slider>().value = volume;

        buttons = new List<Selectable>();
        buttons.Add(Startbuton);
        buttons.Add(Optionbuton);
        buttons.Add(Exitbutton);
        ButtonOn = buttons[0];
        Startbuton.Select();
        currentButtonIndex = 0;
        dPadPressed = false;
	}
    
    public void Exitpress()
    {
        Exitmenu.gameObject.SetActive (true);
        Startbuton.gameObject.SetActive (false);
        Optionbuton.gameObject.SetActive (false);
        Exitbutton.gameObject.SetActive (false);

        buttons.Clear();
        buttons.Add(Exitmenu.transform.GetChild(0).GetChild(1).GetComponent<Button>());
        buttons.Add(Exitmenu.transform.GetChild(0).GetChild(2).GetComponent<Button>());
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    public void Startpress()
    {
        Startmenu.gameObject.SetActive(true);
        Startbuton.gameObject.SetActive(false);
        Optionbuton.gameObject.SetActive(false);
        Exitbutton.gameObject.SetActive(false);

        buttons.Clear();
        buttons.Add(Startmenu.transform.GetChild(0).GetComponent<Button>());
        buttons.Add(Startmenu.transform.GetChild(1).GetComponent<Button>());
        buttons.Add(Startmenu.transform.GetChild(2).GetComponent<Button>());
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    public void Optionpress()
    {
        Optionmenu.gameObject.SetActive(true);
        Startbuton.gameObject.SetActive(false);
        Optionbuton.gameObject.SetActive(false);
        Exitbutton.gameObject.SetActive(false);

        buttons.Clear();
        buttons.Add(Optionmenu.transform.GetChild(0).GetComponent<Slider>());
        buttons.Add(Optionmenu.transform.GetChild(1).GetComponent<Slider>());
        buttons.Add(Optionmenu.transform.GetChild(2).GetComponent<Button>());
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    public void Nopress()
    {
        Exitmenu.gameObject.SetActive(false);
        Startbuton.gameObject.SetActive(true);
        Optionbuton.gameObject.SetActive(true);
        Exitbutton.gameObject.SetActive(true);

        buttons.Clear();
        buttons.Add(Startbuton);
        buttons.Add(Optionbuton);
        buttons.Add(Exitbutton);
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
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

        buttons.Clear();
        buttons.Add(Startbuton);
        buttons.Add(Optionbuton);
        buttons.Add(Exitbutton);
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    public void playerone () 
    {
        SceneManager.LoadScene(1);
    }

    public void playertwo ()
    {
        SceneManager.LoadScene(2);
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && ButtonOn is Button)
            (ButtonOn as Button).onClick.Invoke();

        if (Mathf.Abs(Input.GetAxis("DPadX")) > 0.5f && ButtonOn is Slider)
            (ButtonOn as Slider).value += Input.GetAxis("DPadX") * 0.5f;

        if (!dPadPressed && Input.GetAxis("DPadY") < -0.5f)
        {
            dPadPressed = true;
            currentButtonIndex++;
            if (currentButtonIndex >= buttons.Count)
                currentButtonIndex = 0;
            ButtonOn = buttons[currentButtonIndex];
        }
        else if (!dPadPressed && Input.GetAxis("DPadY") > 0.5f)
        {
            dPadPressed = true;
            currentButtonIndex--;
            if (currentButtonIndex < 0)
                currentButtonIndex = buttons.Count - 1;
            ButtonOn = buttons[currentButtonIndex];
        }
        else if (Mathf.Abs(Input.GetAxis("DPadY")) < 0.3f)
            dPadPressed = false;
	}
}
