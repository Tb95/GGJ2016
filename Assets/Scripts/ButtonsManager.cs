using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonsManager : MonoBehaviour {

	public GameObject redB;
	public GameObject blueB;
	public GameObject greenB;
	public GameObject yellowB;

	public enum Button {leftButton, rightButton, upButton, downButton, redButton, blueButton, yellowButton, greenButton};

	public List<Button> getRandomCombo(int comboLength, InputManager.Side side) {
		List<Button> aList = new List<Button> ();
		for (int i = 0; i < comboLength; i++) {
			aList.Add (getRandomButton (side));
		}
		return aList;
	}

	Button getRandomButton(InputManager.Side side) {
		int a = Random.Range (0, 4);
		if (a == 0) {
			if (side == InputManager.Side.Left) {
				return Button.upButton;
			} else {
				return Button.yellowButton;
			}
		} else if (a == 1) {
			if (side == InputManager.Side.Left) {
				return Button.leftButton;
			} else {
				return Button.redButton;
			}
		} else if (a == 2) {
			if (side == InputManager.Side.Left) {
				return Button.downButton;
			} else {
				return Button.blueButton;
			}
		} else {
			if (side == InputManager.Side.Left) {
				return Button.rightButton;
			} else {
				return Button.greenButton;
			}
		}
	}

	public GameObject getGameObjectFromButton(Button button) {
		GameObject res;
		switch (button) {
		case Button.blueButton:
			res = Instantiate (blueB);
			break;
		case Button.redButton:
			res = Instantiate (redB);
			break;
		case Button.yellowButton:
			res = Instantiate (yellowB);
			break;
		case Button.greenButton:
			res = Instantiate (greenB);
			break;
		default:
			res = Instantiate (blueB);
			break;
		}
		return res;
	}
}
