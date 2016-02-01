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
    [Range(1, 2)]
    public int playerNumber;
	static public int numberOfPlayers;
    
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
    bool dPadPressed;
    string joypadType;

    void Start()
    {
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].StartsWith("Controller (Xbox One"))
            joypadType = "One";
        else
            joypadType = "";

		if (Application.loadedLevel == 2)
			numberOfPlayers = 2;
        else if (Application.loadedLevel == 1)
			numberOfPlayers = 1;

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
            buttons[0] = KeyCode.Joystick2Button0;
            buttons[1] = KeyCode.Joystick2Button1;
            buttons[2] = KeyCode.Joystick2Button2;
            buttons[3] = KeyCode.Joystick2Button3;
            buttons[4] = KeyCode.Joystick2Button4;
            buttons[5] = KeyCode.Joystick2Button5;
            buttons[6] = KeyCode.Joystick2Button6;
            buttons[7] = KeyCode.Joystick2Button7;
        }

        dPadPressed = false;
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
            Input.GetAxis("DPadX" + playerNumber + joypadType) > SENSITIVITY || Input.GetAxis("DPadX" + playerNumber + joypadType) < -SENSITIVITY ||
            Input.GetAxis("DPadY" + playerNumber + joypadType) > SENSITIVITY || Input.GetAxis("DPadY" + playerNumber + joypadType) < -SENSITIVITY ||
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
        if (!dPadPressed && Input.GetAxis("DPadY" + playerNumber + joypadType) < -0.5f)
        {
            dPadPressed = true;

            ButtonTimeSequence.ButtonTime buttonTime;
            buttonTime.button = ButtonsManager.Button.downButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
            buttonTimeSequence.prepend(buttonTime);
        }
        else if (!dPadPressed && Input.GetAxis("DPadY" + playerNumber + joypadType) > 0.5f)
        {
            dPadPressed = true;

            ButtonTimeSequence.ButtonTime buttonTime;
            buttonTime.button = ButtonsManager.Button.upButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
            buttonTimeSequence.prepend(buttonTime);
        }
        else if (!dPadPressed && Input.GetAxis("DPadX" + playerNumber + joypadType) > 0.5f)
        {
            dPadPressed = true;

            ButtonTimeSequence.ButtonTime buttonTime;
            buttonTime.button = ButtonsManager.Button.rightButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
            buttonTimeSequence.prepend(buttonTime);
        }
        else if (!dPadPressed && Input.GetAxis("DPadX" + playerNumber + joypadType) < -0.5f)
        {
            dPadPressed = true;

            ButtonTimeSequence.ButtonTime buttonTime;
            buttonTime.button = ButtonsManager.Button.leftButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
            buttonTimeSequence.prepend(buttonTime);
        }
        else if (Mathf.Abs(Input.GetAxis("DPadY" + playerNumber + joypadType)) < 0.3f && Mathf.Abs(Input.GetAxis("DPadX" + playerNumber + joypadType)) < 0.3f)
            dPadPressed = false;

		if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(buttons[3])) {

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.yellowButton;
			buttonTime.timeFromStart = Time.realtimeSinceStartup;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(buttons[1]))
        {

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.redButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(buttons[0]))
        {

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.greenButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
			buttonTimeSequence.prepend(buttonTime);
        }
        else if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(buttons[2]))
        {

			ButtonTimeSequence.ButtonTime buttonTime;
			buttonTime.button = ButtonsManager.Button.blueButton;
            buttonTime.timeFromStart = Time.realtimeSinceStartup;
			buttonTimeSequence.prepend(buttonTime);
		}
	}

	public static List<SpiderCombo> possibleSpiderCombos = new List<SpiderCombo>();

	void CheckSequences()
	{
		for (int i = 0; i < possibleSpiderCombos.Count; i++) {
            if (buttonTimeSequence.isSequenceOK(possibleSpiderCombos[i], 0.5f * possibleSpiderCombos[i].buttonsList.Count, gameObject))
            {
				// SPIDER EXPLOSION!!!
                possibleSpiderCombos[i].spider.GetSpawner().DeadEnemy();

				// Togli la combo e il trail renderer
				Destroy(possibleSpiderCombos[i].spider.trail.gameObject);
				for (int c = 0; c < possibleSpiderCombos[i].spider.butsPlayer1.Count; c++) {
					Destroy (possibleSpiderCombos[i].spider.butsPlayer1 [c]);
				}
                for (int c = 0; c < possibleSpiderCombos[i].spider.butsPlayer2.Count; c++)
                {
                    Destroy(possibleSpiderCombos[i].spider.butsPlayer2[c]);
                }

				Destroy (possibleSpiderCombos [i].spider.gameObject);
                GetComponent<Health>().DeadEnemy(true, possibleSpiderCombos[i].spider.health);
				possibleSpiderCombos.Remove (possibleSpiderCombos [i]);
				buttonTimeSequence.resetSequence();
			}
		}
	}

    static int leftSpawned = 0;
    static int rightSpawned = 0;
	public static Side getRandomSide() {
        if (Random.Range(0, 100) > leftSpawned / (float)(leftSpawned + rightSpawned) * 100)
        {
            leftSpawned++;
            return Side.Left;
        }
        else
        {
            rightSpawned++;
            return Side.Right;
        }
	}

    public bool isLegalHit(Side spiderSide)
    {
        return currentSidePosition == spiderSide;
    }
}
