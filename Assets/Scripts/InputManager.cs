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
    [Range(1, 4)]
    public int playerNumber;
    
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
    KeyCode[] buttons;

    void Start()
    {
        CurrentSideButton = Side.None;
        currentSidePosition = Side.None;
        player = GetComponent<Player>();
        isPause = false;
        pause = GetComponent<Openpause>();
        nextTwoHands = Time.realtimeSinceStartup;

        buttons = new KeyCode[8];
        if (playerNumber == 1)
        {
            buttons[0] = KeyCode.Joystick1Button0;
            buttons[1] = KeyCode.Joystick1Button1;
            buttons[2] = KeyCode.Joystick1Button2;
            buttons[3] = KeyCode.Joystick1Button3;
            buttons[4] = KeyCode.Joystick1Button4;
            buttons[5] = KeyCode.Joystick1Button5;
            buttons[6] = KeyCode.Joystick1Button6;
            buttons[7] = KeyCode.Joystick1Button7;
        }
        else if (playerNumber == 2)
        {
            buttons[0] = KeyCode.Joystick4Button0;
            buttons[1] = KeyCode.Joystick4Button1;
            buttons[2] = KeyCode.Joystick4Button2;
            buttons[3] = KeyCode.Joystick4Button3;
            buttons[4] = KeyCode.Joystick4Button4;
            buttons[5] = KeyCode.Joystick4Button5;
            buttons[6] = KeyCode.Joystick4Button6;
            buttons[7] = KeyCode.Joystick4Button7;
        }
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

        if (Input.GetAxis("HorizontalL" + playerNumber) > SENSITIVITY || Input.GetAxis("HorizontalL" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("VerticalL" + playerNumber) > SENSITIVITY || Input.GetAxis("VerticalL" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("DPadX" + playerNumber) > SENSITIVITY || Input.GetAxis("DPadX" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("DPadY" + playerNumber) > SENSITIVITY || Input.GetAxis("DPadY" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("TriggerL" + playerNumber) != 0 || Input.GetKey(buttons[4]))
        {
            CurrentSideButton = Side.Left;
            none = false;
        }

        if (Input.GetAxis("HorizontalR" + playerNumber) > SENSITIVITY || Input.GetAxis("HorizontalR" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("VerticalR" + playerNumber) > SENSITIVITY || Input.GetAxis("VerticalR" + playerNumber) < -SENSITIVITY ||
            Input.GetAxis("TriggerR" + playerNumber) != 0 || Input.GetKey(buttons[0]) || Input.GetKey(buttons[1]) ||
            Input.GetKey(buttons[2]) || Input.GetKey(buttons[3]) || Input.GetKey(buttons[5]))
            
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
                direction = new Vector3(Input.GetAxis("HorizontalL" + playerNumber), 0, Input.GetAxis("VerticalL" + playerNumber));
                if (direction.magnitude > SENSITIVITY && currentSidePosition != Side.Right)
                    player.move(direction);
                break;

            case Side.Right:
                direction = new Vector3(Input.GetAxis("HorizontalR" + playerNumber), 0, Input.GetAxis("VerticalR" + playerNumber));
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
                if (currentSidePosition == Side.Left && Input.GetAxis("TriggerL" + playerNumber) > SENSITIVITY)
                    player.attack(CurrentSideButton);
                break;

            case Side.Right:
                if (currentSidePosition == Side.Right && Input.GetAxis("TriggerR" + playerNumber) > SENSITIVITY)
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
            if (hitInfo.collider.tag == "Left1")
                currentSidePosition = Side.Left;
            else if (hitInfo.collider.tag == "Right1")
                currentSidePosition = Side.Right;
        }
        else
            currentSidePosition = Side.None;
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(buttons[7]))
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
		if (Input.GetKeyDown(KeyCode.Z) && Input.GetKeyDown(buttons[3])) {
			Debug.Log("yellow");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.yellowButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.X) && Input.GetKeyDown(buttons[1]))
        {
			Debug.Log("red");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.redButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(buttons[0]))
        {
			Debug.Log("green");

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.greenButton;
			buttonTime.timeFromStart = Time.time;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.V) && Input.GetKeyDown(buttons[2]))
        {
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
                possibleSpiderCombos[i].spider.GetSpawner().DeadEnemy();
				Destroy (possibleSpiderCombos [i].spider.gameObject);
                GetComponent<Health>().DeadEnemy(true);
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
