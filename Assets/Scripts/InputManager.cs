using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    const float SENSITIVITY = 0.3f;

    public enum Side
    {
        None,
        Left,
        Right
    }
    public float twoHandsCooldown;
    
    Side _currentSideButton;
    Side CurrentSideButton
	{
        get { return _currentSideButton; }
        set 
        {
            if (Time.realtimeSinceStartup < nextTwoHands)
                return;

            if (_currentSideButton == Side.None || value == Side.None || currentSidePosition == Side.None)
                _currentSideButton = value;
            else if ((_currentSideButton == Side.Right && value == Side.Left) || (_currentSideButton == Side.Left && value == Side.Right))
            {
                player.Hit(1);
                nextTwoHands = Time.realtimeSinceStartup + twoHandsCooldown;
            }
        }
    }
    Side currentSidePosition;
    Player player;
    bool isPause;
    Openpause pause;
    float nextTwoHands;

    void Start()
    {
        CurrentSideButton = Side.None;
        currentSidePosition = Side.None;
        player = GetComponent<Player>();
        isPause = false;
        pause = GetComponent<Openpause>();
        nextTwoHands = Time.realtimeSinceStartup;
    }

    void Update()
    {
        CheckPosition();
        CheckHand();
        Move();
        Fire();
        CheckPause();
		UpdateSequence();
		CheckSequences ();
    }

    void CheckHand()
    {
        bool none = true;

        if (Input.GetAxis("HorizontalL1") > SENSITIVITY || Input.GetAxis("HorizontalL1") < -SENSITIVITY ||
            Input.GetAxis("VerticalL1") > SENSITIVITY || Input.GetAxis("VerticalL1") < -SENSITIVITY ||
            Input.GetAxis("DPadX") > SENSITIVITY || Input.GetAxis("DPadX") < -SENSITIVITY ||
            Input.GetAxis("DPadY") > SENSITIVITY || Input.GetAxis("DPadY") < -SENSITIVITY ||
            Input.GetAxis("TriggerL") != 0 || Input.GetKey(KeyCode.Joystick1Button4))
        {
            CurrentSideButton = Side.Left;
            none = false;
        }

        if (Input.GetAxis("HorizontalR1") > SENSITIVITY || Input.GetAxis("HorizontalR1") < -SENSITIVITY ||
            Input.GetAxis("VerticalR1") > SENSITIVITY || Input.GetAxis("VerticalR1") < -SENSITIVITY ||
            Input.GetAxis("TriggerR") != 0 || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) ||
            Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) ||Input.GetKey(KeyCode.Joystick1Button5))
            
        { 
            CurrentSideButton = Side.Right;
            none = false;
        }

        if (none)
        {
            CurrentSideButton = Side.None;
        }
    }

    void Move()
    {
        Vector3 direction = Vector3.zero;
        switch (CurrentSideButton)
        {
            case Side.None:
                break;

            case Side.Left:
                direction = new Vector3(Input.GetAxis("HorizontalL1"), 0, Input.GetAxis("VerticalL1"));
                if (direction.magnitude > SENSITIVITY && currentSidePosition != Side.Right)
                    player.move(direction);
                break;

            case Side.Right:
                direction = new Vector3(Input.GetAxis("HorizontalR1"), 0, Input.GetAxis("VerticalR1"));
                if (direction.magnitude > SENSITIVITY  && currentSidePosition != Side.Left)
                    player.move(direction);
                break;
        }
    }

    void Fire()
    {
        switch (CurrentSideButton)
        {
            case Side.None:
                break;

            case Side.Left:
                if (currentSidePosition == Side.Left && Input.GetAxis("TriggerL") > SENSITIVITY)
                    player.attack(CurrentSideButton);
                break;

            case Side.Right:
                if (currentSidePosition == Side.Right && Input.GetAxis("TriggerR") > SENSITIVITY)
                    player.attack(CurrentSideButton);
                break;
        }
    }

    void CheckPosition()
    {
        RaycastHit hitInfo = new RaycastHit();
        int layerMask = 1 << 8;

        if (Physics.Raycast(transform.position, transform.up, out hitInfo, 15, layerMask))
        {
            if (hitInfo.collider.tag == "Left")
                currentSidePosition = Side.Left;
            else if (hitInfo.collider.tag == "Right")
                currentSidePosition = Side.Right;
        }
        else
            currentSidePosition = Side.None;
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if(isPause)
            {
                pause.Resume();
                isPause = false;
            }
            else
            {
                pause.Escpress();
                isPause = true;
            }
        }
    }

	ButtonTimeSequence buttonTimeSequence = new ButtonTimeSequence();

	void UpdateSequence()
	{
		/*
		if (Input.GetButtonDown ("up") && currentSidePosition == Side.Right) {
			Debug.Log ("up");

			ButtonTime buttonTime;
			buttonTime.button = upButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
		} else if (Input.GetButtonDown ("down") && currentSidePosition == Side.Right) {
			Debug.Log ("down");
		} else if (Input.GetButtonDown ("left") && currentSidePosition == Side.Right) {
			Debug.Log ("left");
		} else if (Input.GetButtonDown ("right") && currentSidePosition == Side.Right) {
			Debug.Log ("right");
		}

		if (Input.GetButtonDown ("joystick button 0") && currentSidePosition == Side.Left) {
			Debug.Log ("Button 0");
		} else if (Input.GetButtonDown ("joystick button 1") && currentSidePosition == Side.Left) {
			Debug.Log ("Button 1");
		} else if (Input.GetButtonDown ("joystick button 2") && currentSidePosition == Side.Left) {
			Debug.Log ("Button 2");
		} else if (Input.GetButtonDown ("joystick button 3") && currentSidePosition == Side.Left) {
			Debug.Log ("Button 3");
		}*/
		if (Input.GetButtonDown("yellow")) {
			Debug.Log("yellow");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.yellowButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
		} else if (Input.GetButtonDown("red")) {
			Debug.Log("red");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.redButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
		} else if (Input.GetButtonDown("green")) {
			Debug.Log("green");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.greenButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
		} else if (Input.GetButtonDown("blue")) {
			Debug.Log("blue");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.blueButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
		}
	}

	public List<SpiderCombo> possibleSpiderCombos = new List<SpiderCombo>();

	void CheckSequences()
	{
		for (int i = 0; i < possibleSpiderCombos.Count; i++) {
			if (buttonTimeSequence.isSequenceOK(possibleSpiderCombos[i], 3.0f)) {
				// SPIDER EXPLOSION!!!
				Debug.Log("Tango down!");
				// Togli la combo
				var children = new List<GameObject>();
				foreach (Transform child in possibleSpiderCombos [i].spider.comboText.transform) children.Add(child.gameObject);
				children.ForEach(child => Destroy(child));
				possibleSpiderCombos [i].spider.comboText.SetActive (false);
				Destroy (possibleSpiderCombos [i].spider.gameObject);
				possibleSpiderCombos.Remove (possibleSpiderCombos [i]);
				buttonTimeSequence.resetSequence();
			}
		}
	}

	public Side getRandomSide() {
		if (Random.Range (0, 2) == 0)
			return Side.Left;
		else
			return Side.Right;
	}
}
