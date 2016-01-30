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
                Debug.Log("DUE MANIII!! MUORIIII!!!11!!!11!");
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
