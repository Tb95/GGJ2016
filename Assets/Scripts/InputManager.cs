using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right,
        None
    }
    
    Side _currentSide = Side.None;
    Side CurrentSide
	{
        get { return _currentSide; }
        set 
        {
            if (_currentSide == Side.None)
                _currentSide = value;
            else if (_currentSide != value)
                _currentSide = value;//Player.TwoHands()
        }
    }

    void Update()
    {
        CheckHand();
    }

    void CheckHand()
    {
        if(Input.GetAxis("HorizontalL1") || Input.GetAxis("VerticalL1") || )
    }
}
