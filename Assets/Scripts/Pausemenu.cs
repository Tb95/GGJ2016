using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class Pausemenu : MonoBehaviour {

    public Canvas Gotomainm;
    public Canvas Optionmenu;
    public Canvas Buttoncanvas;
    public Button Resumebutton;
    public Button Optionbutton;
    public Button Mainbutton;
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
        Gotomainm.gameObject.SetActive(false);
        Optionmenu.gameObject.SetActive(false);

        float volume;
        mainMixer.GetFloat("musicVol", out volume);
        Optionmenu.transform.GetChild(0).GetComponent<Slider>().value = volume;
        mainMixer.GetFloat("sfxVol", out volume);
        Optionmenu.transform.GetChild(1).GetComponent<Slider>().value = volume;
	}

    public void Initialize()
    {
        buttons = new List<Selectable>();
        buttons.Add(Resumebutton);
        buttons.Add(Optionbutton);
        buttons.Add(Mainbutton);
        ButtonOn = buttons[0];
        Resumebutton.Select();
        currentButtonIndex = 0;
        dPadPressed = false;
    }
	
    public void Optionpress ()
    {
        Optionmenu.gameObject.SetActive(true);
        Buttoncanvas.gameObject.SetActive(false);

        buttons.Clear();
        buttons.Add(Optionmenu.transform.GetChild(0).GetComponent<Slider>());
        buttons.Add(Optionmenu.transform.GetChild(1).GetComponent<Slider>());
        buttons.Add(Optionmenu.transform.GetChild(2).GetComponent<Button>());
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    public void Mainpress ()
    {
        Gotomainm.gameObject.SetActive(true);
        Resumebutton.gameObject.SetActive(false);
        Optionbutton.gameObject.SetActive(false);
        Mainbutton.gameObject.SetActive(false);

        buttons.Clear();
        buttons.Add(Gotomainm.transform.GetChild(0).GetChild(0).GetComponent<Button>());
        buttons.Add(Gotomainm.transform.GetChild(0).GetChild(1).GetComponent<Button>());
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
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

        buttons.Clear();
        buttons.Add(Resumebutton);
        buttons.Add(Optionbutton);
        buttons.Add(Mainbutton);
        ButtonOn = buttons[0];
        currentButtonIndex = 0;
    }

    void Update()
    {
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
