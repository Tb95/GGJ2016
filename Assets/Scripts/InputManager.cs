using UnityEngine;

public class InputManager : MonoBehaviour
{
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
                Debug.Log("DUE MANIII!! MUORIIII!!!11!!!11!");//Player.TwoHands()
        }
    }

    void Start()
    {
        CurrentSide = Side.None;
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
        Debug.Log(Input.GetAxis("HorizontalL1"));
        if (Input.GetAxis("HorizontalL1") != 0 || Input.GetAxis("VerticalL1") != 0 || Input.GetAxis("DPadX") != 0 ||
            Input.GetAxis("DPadY") != 0 || Input.GetAxis("TriggerL") != 0 || Input.GetKey(KeyCode.Joystick1Button4))
        {
            CurrentSide = Side.Left;
            none = false;
        }

        if (Input.GetAxis("HorizontalR1") > 0.8f || Input.GetAxis("HorizontalR1") < -0.8f || Input.GetAxis("VerticalR1") > 0.8f ||
            Input.GetAxis("VerticalR1") < -0.8f || Input.GetAxis("TriggerR") != 0 || Input.GetKey(KeyCode.Joystick1Button0) ||
            Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) ||
            Input.GetKey(KeyCode.Joystick1Button5))
            
        { 
            CurrentSide = Side.Right;
            none = false;
        }

        if (none)
        {
            CurrentSide = Side.None;
        }

        Debug.Log(CurrentSide);
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
