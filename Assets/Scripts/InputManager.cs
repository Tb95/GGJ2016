using UnityEngine;

public class InputManager : MonoBehaviour
{
    const float SENSITIVITY = 0.3f;

    public enum Side
    {
        None,
        Left,
        Right
    }
    
    Side _currentSide;
    Side CurrentSide
	{
        get { return _currentSide; }
        set 
        {
            if (_currentSide == Side.None || value == Side.None)
                _currentSide = value;
            else if ((_currentSide == Side.Right && value == Side.Left) || (_currentSide == Side.Left && value == Side.Right))
            {
                Debug.Log("DUE MANIII!! MUORIIII!!!11!!!11!");//Player.TwoHands()
                _currentSide = value;
            }
        }
    }
    Player player;

    void Start()
    {
        CurrentSide = Side.None;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        CheckHand();
        //Move();
        //Fire();
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
            CurrentSide = Side.Left;
            none = false;
        }

        if (Input.GetAxis("HorizontalR1") > SENSITIVITY || Input.GetAxis("HorizontalR1") < -SENSITIVITY ||
            Input.GetAxis("VerticalR1") > SENSITIVITY || Input.GetAxis("VerticalR1") < -SENSITIVITY ||
            Input.GetAxis("TriggerR") != 0 || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) ||
            Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) ||Input.GetKey(KeyCode.Joystick1Button5))
            
        { 
            CurrentSide = Side.Right;
            none = false;
        }

        if (none)
        {
            CurrentSide = Side.None;
        }
    }

    void Move()
    {
        Vector3 direction = Vector3.zero;

        switch (CurrentSide)
        {
            case Side.None:
                break;

            case Side.Left:
                direction = new Vector3(Input.GetAxis("HorizontalL1"), 0, Input.GetAxis("VerticalL1"));
                if(direction != Vector3.zero)
                    ;//Player.Move(direction);
                break;

            case Side.Right:
                direction = new Vector3(Input.GetAxis("HorizontalR1"), 0, Input.GetAxis("VerticalR1"));
                if(direction != Vector3.zero)
                    ;//Player.Move(direction);
                break;
        }
    }

    void Fire()
    {
        switch (CurrentSide)
        {
            case Side.None:
                break;

            case Side.Left:
                if (Input.GetButton("4"))
                    ;//Player.Fire(CurrentSide);
                break;

            case Side.Right:
                if (Input.GetButton("5"))
                    ;//Player.Fire(CurrentSide);
                break;
        }
    }
}
